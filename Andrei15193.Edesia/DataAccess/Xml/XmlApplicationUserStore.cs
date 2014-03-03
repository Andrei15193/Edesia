using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlApplicationUserStore
		: IApplicationUserStore
	{
		public XmlApplicationUserStore(XmlDocumentProvider xmlDocumentProvider)
		{
			if (xmlDocumentProvider == null)
				throw new ArgumentNullException("xmlDocumentProvider");
			XmlDocumentFileName = MvcApplication.EdesiaSettings.StorageSettings.MembershipXmlDocumentFileName;
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

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName, _xmlDocumentSchemaSet);
			_ClearTimedoutRegistrationKeys(xmlDocument);
			xmlDocument.Root.AddFirst(_GetUserXElement(applicationUser, password, registrationKey));
			_xmlDocumentProvider.SaveXmlDocument(xmlDocument, _xmlDocumentFileName, _xmlDocumentSchemaSet);
		}
		public ApplicationUser Find(string email, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			IEnumerable<XElement> userXElements = _xmlDocumentProvider.LoadXmlDocument(XmlDocumentFileName, _xmlDocumentSchemaSet)
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
		public void SetAuthenticationToken(ApplicationUser applicationUser, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");
			if (authenticationToken == null)
				throw new ArgumentNullException("authenticationKey");
			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(XmlDocumentFileName, _xmlDocumentSchemaSet);
			XElement userXElement = xmlDocument.Root
											   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
											   .FirstOrDefault(userElement => string.Equals(applicationUser.EMail, userElement.Attribute("EMail").Value, StringComparison.Ordinal));
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
				_xmlDocumentProvider.SaveXmlDocument(xmlDocument, XmlDocumentFileName, _xmlDocumentSchemaSet);
			}
		}
		public void ClearAuthenticationKey(string applicationUserEmail)
		{
			if (applicationUserEmail == null)
				throw new ArgumentNullException("applicationUserEmail");
			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(XmlDocumentFileName, _xmlDocumentSchemaSet);
			XElement userXElement = xmlDocument.Root
											   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
											   .FirstOrDefault(userElement => string.Equals(applicationUserEmail, userElement.Attribute("EMail").Value, StringComparison.Ordinal));
			if (userXElement != null)
			{
				XAttribute authenticationTokenXAttribute = userXElement.Attribute("AuthenticationToken");
				if (authenticationTokenXAttribute != null)
				{
					authenticationTokenXAttribute.Remove();
					_xmlDocumentProvider.SaveXmlDocument(xmlDocument, XmlDocumentFileName, _xmlDocumentSchemaSet);
				}
			}
		}
		public bool ClearRegistrationKey(string userEmail, string userRegistrationKey)
		{
			if (userEmail == null)
				throw new ArgumentNullException("userEmail");
			if (userRegistrationKey == null)
				throw new ArgumentNullException("userRegistrationKey");

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(XmlDocumentFileName, _xmlDocumentSchemaSet);
			_ClearTimedoutRegistrationKeys(xmlDocument);
			XElement userXElement = xmlDocument.Root
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
				_xmlDocumentProvider.SaveXmlDocument(xmlDocument, _xmlDocumentFileName, _xmlDocumentSchemaSet);
				return true;
			}
			return false;
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
					throw new ArgumentNullException("value");
				_xmlDocumentFileName = value;
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
								 XmlConvert.ToDateTime(userXElement.Attribute("RegistrationTime").Value, MvcApplication.DateTimeSerializationFormat));
			XAttribute registrationKeyXAttribute = userXElement.Attribute("RegistrationKey");
			XElement deliveryAddressXElement = userXElement.Element("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}DeliveryAddress");

			if (deliveryAddressXElement != null)
				applicationUser.DeliveryAddress = _GetAddress(deliveryAddressXElement);
			if (registrationKeyXAttribute == null)
				registrationKey = null;
			else
				registrationKey = registrationKeyXAttribute.Value;

			foreach (XElement roleXElement in userXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Role"))
				applicationUser.Roles.Add(_GetRole(roleXElement));

			return applicationUser;
		}
		private XElement _GetUserXElement(ApplicationUser applicationUser, string password, string registrationKey = null)
		{
			XElement newUserXElement = new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser",
													new XAttribute("EMail", applicationUser.EMail),
													new XAttribute("PasswordHash", _ComputeHash(password)));

			if (registrationKey != null)
			{
				newUserXElement.Add(new XAttribute("RegistrationTime", XmlConvert.ToString(applicationUser.RegistrationTime, MvcApplication.DateTimeSerializationFormat)));
				newUserXElement.Add(new XAttribute("RegistrationKey", registrationKey));
			}
			foreach (string userRole in applicationUser.Roles)
				newUserXElement.Add(_GetRoleXElement(userRole));
			if (applicationUser.DeliveryAddress != null)
				newUserXElement.Add(_GetAddressXElement(applicationUser.DeliveryAddress, "DeliveryAddress"));

			return newUserXElement;
		}
		private XElement _GetRoleXElement(string role)
		{
			return new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Role", new XAttribute("Name", role));
		}
		private string _GetRole(XElement roleXElement)
		{
			return roleXElement.Attribute("Name").Value;
		}
		private XElement _GetAddressXElement(Address address)
		{
			return new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}Address",
								new XAttribute("Street", address.Street),
								new XAttribute("City", address.City),
								new XAttribute("County", address.County));
		}
		private XElement _GetAddressXElement(Address address, string xElementName = null)
		{
			return new XElement(("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}" + (xElementName ?? "Address")),
								new XAttribute("Street", address.Street),
								new XAttribute("City", address.City),
								new XAttribute("County", address.County));
		}
		private Address _GetAddress(XElement addressXElement)
		{
			return new Address(addressXElement.Attribute("Street").Value,
							   (City)Enum.Parse(typeof(City), addressXElement.Attribute("City").Value),
							   (County)Enum.Parse(typeof(County), addressXElement.Attribute("City").Value));
		}
		private string _ComputeHash(string authenticationToken)
		{
			return string.Join(string.Empty, _hashAlgorithm.ComputeHash(Encoding.Default.GetBytes(authenticationToken))
														   .Select(hashByte => hashByte.ToString()));
		}
		private void _ClearTimedoutRegistrationKeys(XDocument xmlDocument)
		{
			foreach (XElement timedoutApplicationUser in xmlDocument.Root
																	.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd}ApplicationUser")
																	.Where(userXElement => userXElement.Attribute("RegistrationKey") != null
																						   && (DateTime.Now - XmlConvert.ToDateTime(userXElement.Attribute("RegistrationTime").Value,
																																	MvcApplication.DateTimeSerializationFormat)
																							  ).TotalHours >= MvcApplication.EdesiaSettings.Registration.RegistrationKeyHoursTimeout))
				timedoutApplicationUser.Remove();
		}

		private string _xmlDocumentFileName;
		private readonly HashAlgorithm _hashAlgorithm = new SHA256Managed();
		private readonly XmlDocumentProvider _xmlDocumentProvider;
		private readonly XmlSchemaSet _xmlDocumentSchemaSet;
	}
}