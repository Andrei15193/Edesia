using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlUserStore
		: IUserStore
	{
		public XmlUserStore(IXmlDocumentProvider xmlDocumentProvider)
		{
			if (xmlDocumentProvider == null)
				throw new ArgumentNullException("userStore");
			XmlDocumentFileName = MvcApplication.EdesiaSettings.StorageSettings.MembershipFileName;
			_xmlDocumentProvider = xmlDocumentProvider;
			_xmlDocumentProvider.XmlDocumentSchemaSet.Add(XmlSchema.Read(new StringReader(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<xsd:schema xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
	<xsd:element name=""Membership"">
		<xsd:complexType>
			<xsd:sequence>
				<xsd:element name=""User"" type=""User"" minOccurs=""0"" maxOccurs=""unbounded"">
					<xsd:unique name=""UniqueEmails"">
						<xsd:selector xpath=""User"" />
						<xsd:field xpath=""@EMail"" />
					</xsd:unique>
				</xsd:element>
			</xsd:sequence>
		</xsd:complexType>
	</xsd:element>

	<xsd:complexType name=""User"">
		<xsd:sequence>
			<xsd:element name=""Role"" type=""Role"" minOccurs=""0"" maxOccurs=""unbounded"">
				<xsd:unique  name=""UniqueRoleNames"">
					<xsd:selector xpath=""Role"" />
					<xsd:field xpath=""@Name"" />
				</xsd:unique>
			</xsd:element>
			<xsd:element name=""Address"" type=""Address"" minOccurs=""0"" maxOccurs=""unbounded"" />
		</xsd:sequence>
		<xsd:attribute name=""EMail"" type=""xsd:string"" use=""required"" />
		<xsd:attribute name=""PasswordHash"" type=""xsd:string"" use=""required"" />
		<xsd:attribute name=""RegistrationTime"" type=""xsd:dateTime"" use=""required"" />
		<xsd:attribute name=""AuthenticationToken"" type=""xsd:string"" use=""optional"" />
		<xsd:attribute name=""RegistrationKey"" use=""optional"">
			<xsd:simpleType>
				<xsd:restriction base=""xsd:string"">
					<xsd:maxLength value=""20"" />
				</xsd:restriction>
			</xsd:simpleType>
		</xsd:attribute>
	</xsd:complexType>

	<xsd:complexType name=""Role"">
		<xsd:attribute name=""Name"">
			<xsd:simpleType>
				<xsd:restriction base=""xsd:string"">
					<xsd:enumeration value=""Client"" />
					<xsd:enumeration value=""Angajat"" />
					<xsd:enumeration value=""Administrator"" />
				</xsd:restriction>
			</xsd:simpleType>
		</xsd:attribute>
	</xsd:complexType>

	<xsd:complexType name=""Address"">
		<xsd:attribute name=""Street"" type=""xsd:string"" use=""required"" />
		<xsd:attribute name=""City"" type=""xsd:string"" use=""required"" />
		<xsd:attribute name=""County"" type=""xsd:string"" use=""required"" />
	</xsd:complexType>
</xsd:schema>"), null));
		}

		#region IUserStore Members
		public void AddUser(User user, string password, string registrationKey)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			if (password == null)
				throw new ArgumentNullException("password");
			if (registrationKey == null)
				throw new ArgumentNullException("registrationKey");

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName);
			_ClearTimedoutRegistrationKeys(xmlDocument);
			xmlDocument.Root.AddFirst(_GetUserXElement(user, password, registrationKey));
			_xmlDocumentProvider.SaveXmlDocument(xmlDocument, _xmlDocumentFileName);
		}
		public User Find(string email, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			IEnumerable<XElement> userXElements = _xmlDocumentProvider.LoadXmlDocument(XmlDocumentFileName)
																	  .Root
																	  .Elements("User");
			switch (authenticationTokenType)
			{
				case AuthenticationTokenType.Key:
					return _GetUser(userXElements.FirstOrDefault(userXElement =>
						{
							XAttribute authenticationTokenXAttribute = userXElement.Attribute("AuthenticationToken");
							return (authenticationTokenXAttribute != null && string.Equals(authenticationToken, authenticationTokenXAttribute.Value, StringComparison.Ordinal));
						}));
				case AuthenticationTokenType.Password:
				default:
					string passwordHash = _ComputeHash(authenticationToken);
					return _GetUser(userXElements.FirstOrDefault(userXElement => string.Equals(passwordHash, userXElement.Attribute("PasswordHash").Value, StringComparison.Ordinal)));
			}
		}
		public void SetAuthenticationToken(User user, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			if (authenticationToken == null)
				throw new ArgumentNullException("authenticationKey");
			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(XmlDocumentFileName);
			XElement userXElement = xmlDocument.Root
											   .Elements("User")
											   .FirstOrDefault(userElement => string.Equals(user.EMail, userElement.Attribute("EMail").Value, StringComparison.Ordinal));
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
				_xmlDocumentProvider.SaveXmlDocument(xmlDocument, XmlDocumentFileName);
			}
		}
		public void ClearAuthenticationKey(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(XmlDocumentFileName);
			XElement userXElement = xmlDocument.Root
											   .Elements("User")
											   .FirstOrDefault(userElement => string.Equals(user.EMail, userElement.Attribute("EMail").Value, StringComparison.Ordinal));
			if (userXElement != null)
			{
				XAttribute authenticationTokenXAttribute = userXElement.Attribute("AuthenticationToken");
				if (authenticationTokenXAttribute != null)
				{
					authenticationTokenXAttribute.Remove();
					_xmlDocumentProvider.SaveXmlDocument(xmlDocument, XmlDocumentFileName);
				}
			}
		}
		public bool ClearRegistrationKey(string userEmail, string userRegistrationKey)
		{
			if (userEmail == null)
				throw new ArgumentNullException("userEmail");
			if (userRegistrationKey == null)
				throw new ArgumentNullException("userRegistrationKey");

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(XmlDocumentFileName);
			_ClearTimedoutRegistrationKeys(xmlDocument);
			XElement userXElement = xmlDocument.Root
											   .Elements("User")
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
				_xmlDocumentProvider.SaveXmlDocument(xmlDocument, _xmlDocumentFileName);
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

		private User _GetUser(XElement userXElement)
		{
			// remove, this check is inconsistent
			if (userXElement == null)
				return null;

			string registrationKey;
			User user = _GetUser(userXElement, out registrationKey);

			if (registrationKey != null)
				return null;

			return user;
		}
		private User _GetUser(XElement userXElement, out string registrationKey)
		{
			User user = new User(userXElement.Attribute("EMail").Value,
								 XmlConvert.ToDateTime(userXElement.Attribute("RegistrationTime").Value, MvcApplication.DateTimeSerializationFormat));
			XAttribute registrationKeyXAttribute = userXElement.Attribute("RegistrationKey");

			if (registrationKeyXAttribute == null)
				registrationKey = null;
			else
				registrationKey = registrationKeyXAttribute.Value;

			foreach (XElement roleXElement in userXElement.Elements("Role"))
				user.Roles.Add(_GetRole(roleXElement));
			foreach (XElement addressXElement in userXElement.Elements("Address"))
				user.Addresses.Add(_GetAddress(addressXElement));

			return user;
		}
		private XElement _GetUserXElement(User user, string password, string registrationKey = null)
		{
			XElement newUserXElement = new XElement("User",
													new XAttribute("EMail", user.EMail),
													new XAttribute("PasswordHash", _ComputeHash(password)));

			if (registrationKey != null)
			{
				newUserXElement.Add(new XAttribute("RegistrationTime", XmlConvert.ToString(user.RegistrationTime, MvcApplication.DateTimeSerializationFormat)));
				newUserXElement.Add(new XAttribute("RegistrationKey", registrationKey));
			}
			foreach (string userRole in user.Roles)
				newUserXElement.Add(_GetRoleXElement(userRole));
			foreach (Address userAddress in user.Addresses)
				newUserXElement.Add(_GetAddressXElement(userAddress));

			return newUserXElement;
		}
		private XElement _GetRoleXElement(string role)
		{
			return new XElement("Role", new XAttribute("Name", role));
		}
		private string _GetRole(XElement roleXElement)
		{
			return roleXElement.Attribute("Name").Value;
		}
		private XElement _GetAddressXElement(Address address)
		{
			return new XElement("Address",
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
			foreach (XElement timedoutUser in xmlDocument.Root
														 .Elements("User")
														 .Where(userXElement => userXElement.Attribute("RegistrationKey") != null
																				&& (DateTime.Now - XmlConvert.ToDateTime(userXElement.Attribute("RegistrationTime").Value,
																														 MvcApplication.DateTimeSerializationFormat)
																				   ).TotalHours >= MvcApplication.EdesiaSettings.Registration.RegistrationKeyHoursTimeout))
				timedoutUser.Remove();
		}

		private string _xmlDocumentFileName;
		private readonly IXmlDocumentProvider _xmlDocumentProvider;
		private readonly HashAlgorithm _hashAlgorithm = new SHA256Managed();
	}
}