using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Settings;
using Andrei15193.Edesia.Xml.Validation;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlApplicationUserRepository
		: IApplicationUserRepository
	{
		public XmlApplicationUserRepository(string xmlDocumentFileName, XmlDocumentProvider xmlDocumentProvider)
		{
			if (xmlDocumentFileName == null)
				throw new ArgumentNullException("xmlDocumentFileName");
			if (string.IsNullOrEmpty(xmlDocumentFileName) || string.IsNullOrWhiteSpace(xmlDocumentFileName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentFileName");
			if (xmlDocumentProvider == null)
				throw new ArgumentNullException("xmlDocumentProvider");

			_xmlDocumentFileName = xmlDocumentFileName;
			_xmlDocumentProvider = xmlDocumentProvider;
			_xmlDocumentSchemaSet = new XmlSchemaSet();
			_xmlDocumentSchemaSet.Add("http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd", "http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd");
		}

		#region IApplicationUserProvider Members
		public ApplicationUser GetUser(string eMailAddress, DateTime version)
		{
			if (eMailAddress == null)
				throw new ArgumentNullException("eMailAddress");
			if (string.IsNullOrWhiteSpace(eMailAddress))
				throw new ArgumentException("Cannot be empty or whitespace!", "eMailAddress");

			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(XmlDocumentFileName, version))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationUserXmlElement => string.Equals(applicationUserXmlElement.Attribute("EMail").Value, eMailAddress, StringComparison.Ordinal));

				if (applicationUserXElement == null)
					return null;
				return _TryGetAdministrator(_TryGetEmployee(_GetApplicationUser(applicationUserXElement), applicationUserXElement), applicationUserXElement);
			}
		}
		public ApplicationUser GetUser(string eMailAddress)
		{
			return GetUser(eMailAddress, DateTime.Now);
		}

		public Employee GetEmployee(string eMailAddress, DateTime version)
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, version))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .First(applicationUserXmlElement => string.Equals(applicationUserXmlElement.Attribute("EMail").Value, eMailAddress, StringComparison.Ordinal)
																									 && applicationUserXmlElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Employee") != null);

				if (applicationUserXElement == null)
					return null;
				return (Employee)_TryGetEmployee(_TryGetAdministrator(_GetApplicationUser(applicationUserXElement), applicationUserXElement), applicationUserXElement);
			}
		}
		public Employee GetEmployee(string eMailAddress)
		{
			return GetEmployee(eMailAddress, DateTime.Now);
		}
		public IEnumerable<Employee> GetEmployees()
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
									 .Where(applicationUserXmlElement => applicationUserXmlElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Employee") != null)
									 .Select(applicationUserXmlElement => (Employee)_TryGetEmployee(_TryGetAdministrator(_GetApplicationUser(applicationUserXmlElement), applicationUserXmlElement), applicationUserXmlElement));
		}

		public ShoppingCart GetShoppingCart(ApplicationUser applicationUser, IProductProvider productProvider)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");
			if (productProvider == null)
				throw new ArgumentNullException("productProvider");

			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationXmlElement => string.Equals(applicationXmlElement.Attribute("EMail").Value, applicationUser.EMailAddress, StringComparison.OrdinalIgnoreCase));

				if (applicationUserXElement == null)
					throw new InvalidOperationException("The specified user does not exist!");

				return new ShoppingCart(applicationUser,
										applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ShoppingCart")
															   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Product")
															   .Select(productXElement => _GetOrderedProduct(productXElement, productProvider)));
			}
		}
		#endregion
		#region IApplicationUserRepository Members
		public void AddToCart(ApplicationUser applicationUser, OrderedProduct orderedProduct)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationXmlElement => string.Equals(applicationXmlElement.Attribute("EMail").Value, applicationUser.EMailAddress, StringComparison.OrdinalIgnoreCase));

				if (applicationUserXElement == null)
					throw new InvalidOperationException("The specified user does not exist!");

				XElement shoppingCartXElement = applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ShoppingCart");
				XElement productXElement = shoppingCartXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Product")
															   .FirstOrDefault(productXmlElement => string.Equals(productXmlElement.Attribute("Name").Value, orderedProduct.Product.Name, StringComparison.Ordinal));

				if (productXElement == null)
					shoppingCartXElement.Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Product",
														  new XAttribute("Name", orderedProduct.Product.Name),
														  new XAttribute("Quantity", orderedProduct.Quantity)));
				else
				{
					int quantity = (int.Parse(productXElement.Attribute("Quantity").Value) + orderedProduct.Quantity);

					if (quantity > 0)
						productXElement.Attribute("Quantity").SetValue(quantity);
					else
						productXElement.Remove();
				}

				xmlTransaction.Commit();
			}
		}
		public void UpdateCart(ApplicationUser applicationUser, OrderedProduct orderedProduct)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationXmlElement => string.Equals(applicationXmlElement.Attribute("EMail").Value, applicationUser.EMailAddress, StringComparison.OrdinalIgnoreCase));

				if (applicationUserXElement == null)
					throw new InvalidOperationException("The specified user does not exist!");

				XElement shoppingCartXElement = applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ShoppingCart");
				XElement productXElement = shoppingCartXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Product")
															   .FirstOrDefault(productXmlElement => string.Equals(productXmlElement.Attribute("Name").Value, orderedProduct.Product.Name, StringComparison.Ordinal));

				if (productXElement == null)
				{
					productXElement = new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Product",
												   new XAttribute("Name", orderedProduct.Product.Name),
												   new XAttribute("Quantity", orderedProduct.Quantity));
					shoppingCartXElement.Add(orderedProduct);
				}
				else
					productXElement.Attribute("Quantity").SetValue(orderedProduct.Quantity);

				xmlTransaction.Commit();
			}
		}
		public void RemoveFromCart(ApplicationUser applicationUser, Product product)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");
			if (product == null)
				throw new ArgumentNullException("product");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationXmlElement => string.Equals(applicationXmlElement.Attribute("EMail").Value, applicationUser.EMailAddress, StringComparison.OrdinalIgnoreCase));

				if (applicationUserXElement == null)
					throw new InvalidOperationException("The specified user does not exist!");

				XElement shoppingCartXElement = applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ShoppingCart");
				XElement productXElement = shoppingCartXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Product")
															   .FirstOrDefault(productXmlElement => string.Equals(productXmlElement.Attribute("Name").Value, product.Name, StringComparison.Ordinal));

				if (productXElement != null)
					productXElement.Remove();

				xmlTransaction.Commit();
			}
		}
		public void RemoveFromCarts(Product product)
		{
			if (product == null)
				throw new ArgumentNullException("product");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				xmlTransaction.XmlDocument
							  .Root
							  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
							  .SelectMany(applicationUserXmlElement => applicationUserXmlElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ShoppingCart")
																								.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Product"))
							  .Where(productXmlElement => string.Equals(productXmlElement.Attribute("Name").Value, product.Name, StringComparison.Ordinal))
							  .Remove();

				xmlTransaction.Commit();
			}
		}
		public void ClearShoppingCart(ApplicationUser applicationUser)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationXmlElement => string.Equals(applicationXmlElement.Attribute("EMail").Value, applicationUser.EMailAddress, StringComparison.OrdinalIgnoreCase));

				if (applicationUserXElement == null)
					throw new InvalidOperationException("The specified user does not exist!");

				applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ShoppingCart")
									   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Product")
									   .Remove();

				xmlTransaction.Commit();
			}
		}

		public void AddApplicationUser(ApplicationUser applicationUser, string password, string registrationKey)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");
			if (password == null)
				throw new ArgumentNullException("password");
			if (registrationKey == null)
				throw new ArgumentNullException("registrationKey");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				_ClearTimedoutRegistrationKeys(xmlTransaction.XmlDocument);
				xmlTransaction.XmlDocument
							  .Root
							  .AddFirst(_GetApplicationUserXElement(applicationUser, password, registrationKey));

				xmlTransaction.Commit();
			}
		}
		
		public void EnrollAdministrator(string eMailAddress)
		{
			if (eMailAddress == null)
				throw new ArgumentNullException("eMailAddress");
			if (string.IsNullOrWhiteSpace(eMailAddress))
				throw new ArgumentException("Cannot be empty or whitespace!", "eMailAddress");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(XmlDocumentFileName))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationUserXmlElement => string.Equals(applicationUserXmlElement.Attribute("EMail").Value, eMailAddress, StringComparison.Ordinal));

				if (applicationUserXElement != null && applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Administrator") == null)
				{
					applicationUserXElement.Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Administrator"));
					xmlTransaction.Commit();
				}
			}
		}
		public void EnrollEmployee(string eMailAddress, double transportCapacity)
		{
			if (eMailAddress == null)
				throw new ArgumentNullException("eMailAddress");
			if (string.IsNullOrWhiteSpace(eMailAddress))
				throw new ArgumentException("Cannot be empty or whitespace!", "eMailAddress");

			if (transportCapacity <= 0)
				throw new ArgumentNullException("Must be strictly positive!", "transportCapacity");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(XmlDocumentFileName))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationUserXmlElement => string.Equals(applicationUserXmlElement.Attribute("EMail").Value, eMailAddress, StringComparison.Ordinal));

				if (applicationUserXElement != null && applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Employee") == null)
				{
					applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ShoppingCart")
										   .AddAfterSelf(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Administrator",
																	  new XAttribute("TransportCapacity", transportCapacity)));
					xmlTransaction.Commit();
				}
			}
		}

		public ApplicationUser Find(string eMail, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			if (eMail == null)
				throw new ArgumentNullException("eMail");
			if (authenticationToken == null)
				throw new ArgumentNullException("authenticationToken");

			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement applicationUserXElement = xmlTransaction.XmlDocument
																 .Root
																 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																 .FirstOrDefault(applicationUserXmlElement => string.Equals(applicationUserXmlElement.Attribute("EMail").Value, eMail, StringComparison.OrdinalIgnoreCase));
				if (applicationUserXElement == null)
					return null;

				switch (authenticationTokenType)
				{
					case AuthenticationTokenType.Key:
						XAttribute authenticationTokenXAttribute = applicationUserXElement.Attribute("AuthenticationToken");

						if (authenticationTokenXAttribute == null || !string.Equals(authenticationToken, authenticationTokenXAttribute.Value, StringComparison.Ordinal))
							return null;
						break;
					case AuthenticationTokenType.Password:
					default:
						string passwordHash = _ComputeHash(authenticationToken);

						if (!string.Equals(passwordHash, applicationUserXElement.Attribute("PasswordHash").Value, StringComparison.Ordinal))
							return null;
						break;
				}

				return _TryGetAdministrator(_TryGetEmployee(_GetApplicationUser(applicationUserXElement), applicationUserXElement), applicationUserXElement);
			}
		}
		public void SetAuthenticationToken(ApplicationUser applicationUser, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");
			if (authenticationToken == null)
				throw new ArgumentNullException("authenticationKey");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement userXElement = xmlTransaction.XmlDocument
													  .Root
													  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
													  .FirstOrDefault(userElement => string.Equals(applicationUser.EMailAddress, userElement.Attribute("EMail").Value, StringComparison.Ordinal));
				if (userXElement != null)
				{
					switch (authenticationTokenType)
					{
						case AuthenticationTokenType.Key:
							XAttribute authenticationTokenXAttribute = userXElement.Attribute("AuthenticationToken");
							if (authenticationTokenXAttribute != null)
								authenticationTokenXAttribute.SetValue(authenticationToken);
							else
								userXElement.Add(new XAttribute("AuthenticationToken", authenticationToken));
							break;
						case AuthenticationTokenType.Password:
						default:
							userXElement.Attribute("Password").SetValue(_ComputeHash(authenticationToken));
							break;
					}
					try
					{
						xmlTransaction.Commit();
					}
					catch (AggregateException xmlExceptions)
					{
						throw new AggregateException(xmlExceptions.InnerExceptions.Select(_TranslateException));
					}
				}
			}
		}
		public void ClearAuthenticationKey(string applicationUserEmail)
		{
			if (applicationUserEmail == null)
				throw new ArgumentNullException("applicationUserEmail");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement userXElement = xmlTransaction.XmlDocument
													  .Root
													  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
													  .FirstOrDefault(userElement => string.Equals(applicationUserEmail, userElement.Attribute("EMail").Value, StringComparison.Ordinal));
				if (userXElement != null)
				{
					XAttribute authenticationTokenXAttribute = userXElement.Attribute("AuthenticationToken");
					if (authenticationTokenXAttribute != null)
					{
						authenticationTokenXAttribute.Remove();
						try
						{
							xmlTransaction.Commit();
						}
						catch (AggregateException xmlExceptions)
						{
							throw new AggregateException(xmlExceptions.InnerExceptions.Select(_TranslateException));
						}
					}
				}
			}
		}
		public bool ClearRegistrationKey(string userEmail, string userRegistrationKey)
		{
			if (userEmail == null)
				throw new ArgumentNullException("userEmail");
			if (userRegistrationKey == null)
				throw new ArgumentNullException("userRegistrationKey");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				_ClearTimedoutRegistrationKeys(xmlTransaction.XmlDocument);

				XElement userXElement = xmlTransaction.XmlDocument
													  .Root
													  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
													  .FirstOrDefault(userElement =>
																	  {
																		  XAttribute registrationKeyXAttribute = userElement.Attribute("RegistrationKey");
																		  return (registrationKeyXAttribute != null
																				  && string.Equals(userEmail, userElement.Attribute("EMail").Value, StringComparison.Ordinal)
																				  && string.Equals(userRegistrationKey, registrationKeyXAttribute.Value, StringComparison.Ordinal));
																	  });
				if (userXElement != null)
				{
					userXElement.Attribute("RegistrationKey").Remove();
					try
					{
						xmlTransaction.Commit();
					}
					catch (AggregateException xmlExceptions)
					{
						throw new AggregateException(xmlExceptions.InnerExceptions.Select(_TranslateException));
					}

					return true;
				}
				return false;
			}
		}
		#endregion
		public string XmlDocumentFileName
		{
			get
			{
				return _xmlDocumentFileName;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("XmlDocumentFileName");
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Cannot be empty or whitespace!", "XmlDocumentFileName");

				_xmlDocumentFileName = value;
			}
		}
		public XmlDocumentProvider XmlDocumentProvider
		{
			get
			{
				return _xmlDocumentProvider;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("XmlDocumentProvider");

				_xmlDocumentProvider = value;
			}
		}

		private ApplicationUser _TryGetAdministrator(ApplicationUser applicationUser, XElement applicationUserXElement)
		{
			XElement administratorXElement = applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Administrator");

			if (administratorXElement != null)
				return new Administrator(applicationUser);
			else
				return applicationUser;
		}
		private ApplicationUser _TryGetEmployee(ApplicationUser applicationUser, XElement applicationUserXElement)
		{
			XElement employeeXElement = applicationUserXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Employee");

			if (employeeXElement != null)
				return new Employee(applicationUser, double.Parse(employeeXElement.Attribute("TransportCapacity").Value));
			else
				return applicationUser;
		}
		private ApplicationUser _GetApplicationUser(XElement applicationUserXElement)
		{
			string registrationKey;
			ApplicationUser applicationUser = _GetApplicationUser(applicationUserXElement, out registrationKey);

			if (registrationKey != null)
				return null;

			return applicationUser;
		}
		private ApplicationUser _GetApplicationUser(XElement userXElement, out string registrationKey)
		{
			ApplicationUser applicationUser = new ApplicationUser(userXElement.Attribute("EMail").Value,
																  userXElement.Attribute("FirstName").Value,
																  userXElement.Attribute("LastName").Value,
																  XmlConvert.ToDateTime(userXElement.Attribute("RegistrationTime").Value, MvcApplication.DateTimeSerializationFormat));
			XAttribute registrationKeyXAttribute = userXElement.Attribute("RegistrationKey");

			if (registrationKeyXAttribute != null)
				registrationKey = registrationKeyXAttribute.Value;
			else
				registrationKey = null;

			return applicationUser;
		}
		private XElement _GetApplicationUserXElement(ApplicationUser applicationUser, string password, string registrationKey = null)
		{
			XElement applicationUserXElement = new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser",
															new XAttribute("EMail", applicationUser.EMailAddress),
															new XAttribute("FirstName", applicationUser.FirstName),
															new XAttribute("LastName", applicationUser.LastName),
															new XAttribute("PasswordHash", _ComputeHash(password)),
															new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ShoppingCart"));

			if (registrationKey != null)
			{
				applicationUserXElement.Add(new XAttribute("RegistrationTime", XmlConvert.ToString(applicationUser.RegistrationTime, MvcApplication.DateTimeSerializationFormat)));
				applicationUserXElement.Add(new XAttribute("RegistrationKey", registrationKey));
			}

			ApplicationUserRole applicationUserRole = applicationUser as ApplicationUserRole;

			if (applicationUserRole != null)
			{
				Employee employee = applicationUserRole.TryGetRole<Employee>();

				if (employee != null)
					applicationUserXElement.Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Employee",
															 new XAttribute("TransportCapacity", employee.TransportCapacity)));

				Administrator administrator = applicationUserRole.TryGetRole<Administrator>();

				if (administrator != null)
					applicationUserXElement.Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Administrator"));
			}

			return applicationUserXElement;
		}

		private OrderedProduct _GetOrderedProduct(XElement productXElement, IProductProvider productProvider)
		{
			return new OrderedProduct(productProvider.GetProduct(productXElement.Attribute("Name").Value),
									  int.Parse(productXElement.Attribute("Quantity").Value));
		}

		private string _ComputeHash(string authenticationToken)
		{
			return string.Join(string.Empty, _hashAlgorithm.ComputeHash(Encoding.Default.GetBytes(authenticationToken))
														   .Select(hashByte => hashByte.ToString()));
		}
		private void _ClearTimedoutRegistrationKeys(XDocument xmlDocument)
		{
			IRegistrationSettings registrationSettings = (IRegistrationSettings)MvcApplication.DependencyContainer["registrationSettings"];

			foreach (XElement timedoutApplicationUser in xmlDocument.Root
																	.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																	.Where(userXElement => userXElement.Attribute("RegistrationKey") != null
																						   && (DateTime.Now - XmlConvert.ToDateTime(userXElement.Attribute("RegistrationTime").Value,
																																	MvcApplication.DateTimeSerializationFormat)
																							  ).TotalHours >= registrationSettings.RegistrationKeyHoursTimeout))
				timedoutApplicationUser.Remove();
		}
		private Exception _TranslateException(Exception exception)
		{
			XmlUniqueConstraintException xmlUniqueConstraintException = exception as XmlUniqueConstraintException;

			if (xmlUniqueConstraintException != null)
			{
				if (string.Equals("http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd:UniqueEMails", xmlUniqueConstraintException.ConstraintName, StringComparison.Ordinal))
					return new UniqueEMailAddressException(xmlUniqueConstraintException.ConflictingValue, xmlUniqueConstraintException);

				if (string.Equals("http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd:UniqueDeliveryZoneNames", xmlUniqueConstraintException.ConstraintName, StringComparison.Ordinal))
					return new UniqueDeliveryZoneNameException(xmlUniqueConstraintException.ConflictingValue, xmlUniqueConstraintException);
			}

			return exception;
		}

		private string _xmlDocumentFileName;
		private XmlDocumentProvider _xmlDocumentProvider;
		private readonly HashAlgorithm _hashAlgorithm = new SHA256Managed();
		private readonly XmlSchemaSet _xmlDocumentSchemaSet;
	}
}