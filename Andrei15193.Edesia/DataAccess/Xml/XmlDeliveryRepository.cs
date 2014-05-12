using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Xml.Validation;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlDeliveryRepository
		: IDeliveryRepository
	{
		public XmlDeliveryRepository(string xmlDocumentFileName, XmlDocumentProvider xmlDocumentProvider)
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
			_xmlDocumentSchemaSet.Add("http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd", "http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd");
		}

		#region IDeliveryRepository Members
		public IEnumerable<string> GetUnmappedAddresses()
		{
			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
									 .Select(addressXElement => addressXElement.Value);
		}
		public IEnumerable<DeliveryZone> GetDeliveryZones()
		{
			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}DeliveryZone")
									 .Select(deliveryZoneXElement => new DeliveryZone(deliveryZoneXElement.Attribute("Name").Value,
																					  Colour.Parse(deliveryZoneXElement.Attribute("Colour").Value),
																					  deliveryZoneXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address").Select(addressXElement => addressXElement.Value)));
		}
		public void AddAddress(string addressName)
		{
			if (addressName == null)
				throw new ArgumentNullException("addressName");
			if (string.IsNullOrEmpty(addressName) || string.IsNullOrWhiteSpace(addressName))
				throw new ArgumentException("Cannot be empty or whitespace.", "addressName");

			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				xmlTransaction.XmlDocument
							  .Root
							  .Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address", addressName));

				xmlTransaction.Commit();
			}
		}
		public void RemoveAddress(string addressName)
		{
			if (addressName == null)
				throw new ArgumentNullException("addressName");
			if (string.IsNullOrEmpty(addressName) || string.IsNullOrWhiteSpace(addressName))
				throw new ArgumentException("Cannot be empty or whitespace.", "addressName");

			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement addressXElement = xmlTransaction.XmlDocument
														 .Root
														 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
														 .FirstOrDefault(addressXmlElement => string.Equals(addressXmlElement.Value, addressName, StringComparison.OrdinalIgnoreCase));

				if (addressXElement != null)
				{
					addressXElement.Remove();
					xmlTransaction.Commit();
				}
			}
		}
		public void AddDeliveryZone(DeliveryZone deliveryZone)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				xmlTransaction.XmlDocument
							  .Root
							  .AddFirst(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}DeliveryZone",
													 new XAttribute("Name", deliveryZone.Name),
													 new XAttribute("Colour", deliveryZone.Colour.ToString()),
													 deliveryZone.Addresses.Select(address => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address", address))));

				xmlTransaction.XmlDocument
							  .Root
							  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
							  .Where(addressXElement => deliveryZone.Addresses.Contains(addressXElement.Value))
							  .Remove();

				xmlTransaction.Commit();
			}
		}
		public void UpdateDeliveryZone(DeliveryZone deliveryZone, string deliveryZoneOldName)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			if (deliveryZoneOldName == null)
				throw new ArgumentNullException("deliveryZoneOldName");
			if (string.IsNullOrEmpty(deliveryZoneOldName) || string.IsNullOrWhiteSpace(deliveryZoneOldName))
				throw new ArgumentException("Cannot be empty or whitespace.", "deliveryZoneOldName");

			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement deliveryZoneXElement = xmlTransaction.XmlDocument
															  .Root
															  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}DeliveryZone")
															  .FirstOrDefault(deliveryZoneXmlElement => string.Equals(deliveryZoneXmlElement.Attribute("Name").Value, deliveryZoneOldName, StringComparison.OrdinalIgnoreCase));

				if (deliveryZoneXElement != null)
				{
					xmlTransaction.XmlDocument
								  .Root
								  .Add(deliveryZoneXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
														   .Select(addressXElement => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address", addressXElement.Value)));

					deliveryZoneXElement.Attribute("Name").SetValue(deliveryZone.Name);
					deliveryZoneXElement.Attribute("Colour").SetValue(deliveryZone.Colour.ToString());
					deliveryZoneXElement.Elements().Remove();
					deliveryZoneXElement.Add(deliveryZone.Addresses.Select(address => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address", address)));

					xmlTransaction.XmlDocument
								  .Root
								  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
								  .Where(addressXElement => deliveryZone.Addresses.Contains(addressXElement.Value))
								  .Remove();

					xmlTransaction.Commit();
				}
			}
		}
		public void RemoveDeliveryZone(string deliveryZoneName)
		{
			if (deliveryZoneName == null)
				throw new ArgumentNullException("deliveryZoneName");
			if (string.IsNullOrEmpty(deliveryZoneName) || string.IsNullOrWhiteSpace(deliveryZoneName))
				throw new ArgumentException("Cannot be empty or whitespace.", "deliveryZoneName");

			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement deliveryZoneXElement = xmlTransaction.XmlDocument
															  .Root
															  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}DeliveryZone")
															  .FirstOrDefault(deliveryZoneXmlElement => string.Equals(deliveryZoneXmlElement.Attribute("Name").Value, deliveryZoneName, StringComparison.OrdinalIgnoreCase));

				if (deliveryZoneXElement != null)
				{
					xmlTransaction.XmlDocument
								  .Root
								  .Add(deliveryZoneXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address"));
					deliveryZoneXElement.Remove();

					xmlTransaction.Commit();
				}
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
				if (string.Equals("http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd:UniqueAddresses", xmlUniqueConstraintException.ConstraintName, StringComparison.Ordinal))
					return new UniqueAddressException(xmlUniqueConstraintException.ConflictingValue, xmlUniqueConstraintException);

				if (string.Equals("http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd:UniqueDeliveryZoneNames", xmlUniqueConstraintException.ConstraintName, StringComparison.Ordinal))
					return new UniqueDeliveryZoneNameException(xmlUniqueConstraintException.ConflictingValue, xmlUniqueConstraintException);
			}

			return exception;
		}

		private string _xmlDocumentFileName;
		private XmlDocumentProvider _xmlDocumentProvider;
		private readonly XmlSchemaSet _xmlDocumentSchemaSet;
	}
}