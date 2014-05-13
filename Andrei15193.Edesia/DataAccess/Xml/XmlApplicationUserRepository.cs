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

		#region IUserStore Members
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

		public ApplicationUser Find(string email, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				IEnumerable<XElement> userXElements = xmlTransaction.XmlDocument
																	.Root
																	.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser");
				switch (authenticationTokenType)
				{
					case AuthenticationTokenType.Key:
						return _GetApplicationUser(userXElements.FirstOrDefault(userXElement =>
							{
								XAttribute authenticationTokenXAttribute = userXElement.Attribute("AuthenticationToken");
								return (authenticationTokenXAttribute != null && string.Equals(authenticationToken, authenticationTokenXAttribute.Value, StringComparison.Ordinal));
							}));
					case AuthenticationTokenType.Password:
					default:
						string passwordHash = _ComputeHash(authenticationToken);
						return _GetApplicationUser(userXElements.FirstOrDefault(userXElement => string.Equals(passwordHash, userXElement.Attribute("PasswordHash").Value, StringComparison.Ordinal)));
				}
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

					xmlTransaction.Commit();
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
						xmlTransaction.Commit();
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
					xmlTransaction.Commit();

					return true;
				}
				return false;
			}
		}
		public IEnumerable<DetailedAddress> GetAddresses()
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
									 .SelectMany(applicationUserXElement => applicationUserXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Address"))
									 .Select(_GetAddress);
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

		private ApplicationUser _GetApplicationUser(XElement userXElement)
		{
			// remove, this check is inconsistent
			if (userXElement == null)
				return null;

			string registrationKey;
			ApplicationUser applicationUser = _GetApplicationUser(userXElement, out registrationKey);

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

			if (registrationKeyXAttribute == null)
				registrationKey = null;
			else
				registrationKey = registrationKeyXAttribute.Value;

			XElement addressXElement = userXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Address");
			if (addressXElement != null)
				applicationUser.DetailedAddress = _GetAddress(addressXElement);

			XElement employeeXElement = userXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Employee");
			if (employeeXElement != null)
			{
				Employee employee = new Employee(applicationUser, int.Parse(employeeXElement.Attribute("TransportCapacity").Value));

				foreach (XElement deliveryZoneXElement in employeeXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}DeliveryZone"))
					employee.DeliveryZones.Add(deliveryZoneXElement.Value);

				applicationUser = employee;
			}

			XElement administratorXElement = userXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Administrator");
			if (administratorXElement != null)
				applicationUser = new Administrator(applicationUser);

			return applicationUser;
		}
		private XElement _GetApplicationUserXElement(ApplicationUser applicationUser, string password, string registrationKey = null)
		{
			XElement applicationUserXElement = new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser",
															new XAttribute("EMail", applicationUser.EMailAddress),
															new XAttribute("FirstName", applicationUser.FirstName),
															new XAttribute("LastName", applicationUser.LastName),
															new XAttribute("PasswordHash", _ComputeHash(password)));

			if (registrationKey != null)
			{
				applicationUserXElement.Add(new XAttribute("RegistrationTime", XmlConvert.ToString(applicationUser.RegistrationTime, MvcApplication.DateTimeSerializationFormat)));
				applicationUserXElement.Add(new XAttribute("RegistrationKey", registrationKey));
			}
			if (applicationUser.DetailedAddress != null)
				applicationUserXElement.Add(_GetAddressXElement(applicationUser.DetailedAddress));

			ApplicationUserRole applicationUserRole = applicationUser as ApplicationUserRole;

			if (applicationUserRole != null)
			{
				Employee employee = applicationUserRole.TryGetRole<Employee>();

				if (employee != null)
					applicationUserXElement.Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Employee",
															 employee.DeliveryZones.Select(deliveryZone => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}DeliveryZone", deliveryZone))));

				Administrator administrator = applicationUserRole.TryGetRole<Administrator>();

				if (administrator != null)
					applicationUserXElement.Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Administrator"));
			}

			return applicationUserXElement;
		}
		private XElement _GetAddressXElement(DetailedAddress address)
		{
			if (address.Details == null)
				return new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}DetailedAddress",
									new XAttribute("Address", address.Address));
			else
				return new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}DetailedAddress",
									new XAttribute("Address", address.Address),
									new XAttribute("Details", address.Details));
		}
		private DetailedAddress _GetAddress(XElement detailedAddressXElement)
		{
			XAttribute addressDetailsAttribute = detailedAddressXElement.Attribute("Details");

			if (addressDetailsAttribute == null)
				return new DetailedAddress(detailedAddressXElement.Attribute("Address").Value);
			else
				return new DetailedAddress(detailedAddressXElement.Attribute("Address").Value, addressDetailsAttribute.Value);
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
		[Obsolete]
		private void _SaveXmlDocument(XDocument xmlDocument)
		{
			try
			{
				_xmlDocumentProvider.SaveXmlDocument(xmlDocument, _xmlDocumentFileName, _xmlDocumentSchemaSet);
			}
			catch (AggregateException xmlExceptions)
			{
				throw new AggregateException(xmlExceptions.InnerExceptions.Select(_TranslateException));
			}
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