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
			return _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName)
									   .Root
									   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
									   .Select(addressXElement => addressXElement.Value);
		}
		public IEnumerable<DeliveryZone> GetDeliveryZones()
		{
			return _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName)
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

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName);
			xmlDocument.Root.Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address", addressName));

			_SaveXmlDocument(xmlDocument);
		}
		public void RemoveAddress(string addressName)
		{
			if (addressName == null)
				throw new ArgumentNullException("addressName");
			if (string.IsNullOrEmpty(addressName) || string.IsNullOrWhiteSpace(addressName))
				throw new ArgumentException("Cannot be empty or whitespace.", "addressName");

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName);
			XElement addressXElement = xmlDocument.Root
												  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
												  .FirstOrDefault(addressXmlElement => string.Equals(addressXmlElement.Value, addressName, StringComparison.OrdinalIgnoreCase));

			if (addressXElement != null)
			{
				addressXElement.Remove();
				_SaveXmlDocument(xmlDocument);
			}
		}
		public void AddDeliveryZone(DeliveryZone deliveryZone)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName);

			xmlDocument.Root.AddFirst(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}DeliveryZone",
												   new XAttribute("Name", deliveryZone.Name),
												   new XAttribute("Colour", deliveryZone.Colour.ToString()),
												   deliveryZone.Addresses.Select(address => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address", address))));

			xmlDocument.Root
					   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
					   .Where(addressXElement => deliveryZone.Addresses.Contains(addressXElement.Value))
					   .Remove();

			_SaveXmlDocument(xmlDocument);
		}
		public void UpdateDeliveryZone(DeliveryZone deliveryZone, string deliveryZoneOldName)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			if (deliveryZoneOldName == null)
				throw new ArgumentNullException("deliveryZoneOldName");
			if (string.IsNullOrEmpty(deliveryZoneOldName) || string.IsNullOrWhiteSpace(deliveryZoneOldName))
				throw new ArgumentException("Cannot be empty or whitespace.", "deliveryZoneOldName");

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName);

			XElement deliveryZoneXElement = xmlDocument.Root
													   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}DeliveryZone")
													   .FirstOrDefault(deliveryZoneXmlElement => string.Equals(deliveryZoneXmlElement.Attribute("Name").Value, deliveryZoneOldName, StringComparison.OrdinalIgnoreCase));

			if (deliveryZoneXElement != null)
			{
				xmlDocument.Root.Add(deliveryZoneXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
														 .Select(addressXElement => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address", addressXElement.Value)));

				deliveryZoneXElement.Attribute("Name").SetValue(deliveryZone.Name);
				deliveryZoneXElement.Attribute("Colour").SetValue(deliveryZone.Colour.ToString());
				deliveryZoneXElement.Elements().Remove();
				deliveryZoneXElement.Add(deliveryZone.Addresses.Select(address => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address", address)));

				xmlDocument.Root
						   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address")
						   .Where(addressXElement => deliveryZone.Addresses.Contains(addressXElement.Value))
						   .Remove();

				_SaveXmlDocument(xmlDocument);
			}
		}
		public void RemoveDeliveryZone(string deliveryZoneName)
		{
			if (deliveryZoneName == null)
				throw new ArgumentNullException("deliveryZoneName");
			if (string.IsNullOrEmpty(deliveryZoneName) || string.IsNullOrWhiteSpace(deliveryZoneName))
				throw new ArgumentException("Cannot be empty or whitespace.", "deliveryZoneName");

			XDocument xmlDocument = _xmlDocumentProvider.LoadXmlDocument(_xmlDocumentFileName);

			XElement deliveryZoneXElement = xmlDocument.Root
													   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}DeliveryZone")
													   .FirstOrDefault(deliveryZoneXmlElement => string.Equals(deliveryZoneXmlElement.Attribute("Name").Value, deliveryZoneName, StringComparison.OrdinalIgnoreCase));

			if (deliveryZoneXElement != null)
			{
				xmlDocument.Root.Add(deliveryZoneXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd}Address"));
				deliveryZoneXElement.Remove();

				_SaveXmlDocument(xmlDocument);
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