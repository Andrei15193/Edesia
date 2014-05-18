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
			_xmlDocumentSchemaSet.Add("http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd", "http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd");
		}

		#region IDeliveryRepository Members
		public IEnumerable<string> GetUnmappedAddresses()
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
									 .Select(addressXElement => addressXElement.Value);
		}
		public IEnumerable<DeliveryZone> GetDeliveryZones(IApplicationUserProvider applicationUserProvider)
		{
			if (applicationUserProvider == null)
				throw new ArgumentNullException("applicationUserProvider");

			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}DeliveryZone")
									 .Select(deliveryZoneXmlElement => _GetDeliveryZone(deliveryZoneXmlElement, applicationUserProvider));
		}
		public void AddAddress(string addressName)
		{
			if (addressName == null)
				throw new ArgumentNullException("addressName");
			if (string.IsNullOrEmpty(addressName) || string.IsNullOrWhiteSpace(addressName))
				throw new ArgumentException("Cannot be empty or whitespace.", "addressName");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				xmlTransaction.XmlDocument
							  .Root
							  .Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address", addressName));
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
		public void RemoveAddress(string addressName)
		{
			if (addressName == null)
				throw new ArgumentNullException("addressName");
			if (string.IsNullOrEmpty(addressName) || string.IsNullOrWhiteSpace(addressName))
				throw new ArgumentException("Cannot be empty or whitespace.", "addressName");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement addressXElement = xmlTransaction.XmlDocument
														 .Root
														 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
														 .FirstOrDefault(addressXmlElement => string.Equals(addressXmlElement.Value, addressName, StringComparison.OrdinalIgnoreCase)
																							  && !addressXmlElement.Elements().Any());

				if (addressXElement != null)
				{
					addressXElement.Remove();
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
		public void AddDeliveryZone(DeliveryZone deliveryZone)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement deliveryZoneXElement = new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}DeliveryZone",
															 new XAttribute("Name", deliveryZone.Name),
															 new XAttribute("Colour", deliveryZone.Colour.ToString()),
															 xmlTransaction.XmlDocument
																		   .Root
																		   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
																		   .Where(unmappedAddressXElement => deliveryZone.Addresses.Contains(unmappedAddressXElement.Value)));

				if (deliveryZone.Assignee != null)
					deliveryZoneXElement.Add(new XAttribute("AssigneeEMailAddress", deliveryZone.Assignee.EMailAddress));

				xmlTransaction.XmlDocument
							  .Root
							  .AddFirst(deliveryZoneXElement);
				xmlTransaction.XmlDocument
							  .Root
							  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
							  .Where(unmappedAddressXElement => deliveryZone.Addresses.Contains(unmappedAddressXElement.Value))
							  .Remove();
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
		public void UpdateDeliveryZone(DeliveryZone deliveryZone, string deliveryZoneOldName)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			if (deliveryZoneOldName == null)
				throw new ArgumentNullException("deliveryZoneOldName");
			if (string.IsNullOrEmpty(deliveryZoneOldName) || string.IsNullOrWhiteSpace(deliveryZoneOldName))
				throw new ArgumentException("Cannot be empty or whitespace.", "deliveryZoneOldName");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement deliveryZoneXElement = xmlTransaction.XmlDocument
															  .Root
															  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}DeliveryZone")
															  .FirstOrDefault(deliveryZoneXmlElement => string.Equals(deliveryZoneXmlElement.Attribute("Name").Value, deliveryZoneOldName, StringComparison.OrdinalIgnoreCase));

				if (deliveryZoneXElement != null)
				{
					deliveryZoneXElement.Add(xmlTransaction.XmlDocument
														   .Root
														   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
														   .Where(unmappedAddressXElement => deliveryZone.Addresses.Contains(unmappedAddressXElement.Value)));

					XAttribute assigneeXAttribute = deliveryZoneXElement.Attribute("AssigneeEMailAddress");
					if (deliveryZone.Assignee != null)
						if (assigneeXAttribute == null)
							deliveryZoneXElement.Add(new XAttribute("AssigneeEMailAddress", deliveryZone.Assignee.EMailAddress));
						else
							assigneeXAttribute.SetValue(deliveryZone.Assignee.EMailAddress);
					else
						if (assigneeXAttribute != null)
							assigneeXAttribute.Remove();

					xmlTransaction.XmlDocument
								  .Root
								  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
								  .Where(unmappedAddressXElement => deliveryZone.Addresses.Contains(unmappedAddressXElement.Value))
								  .Remove();

					xmlTransaction.XmlDocument
								  .Root
								  .Add(deliveryZoneXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
														   .Where(unmappedAddressXElement => !deliveryZone.Addresses.Contains(unmappedAddressXElement.Value)));
					deliveryZoneXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
										.Where(unmappedAddressXElement => !deliveryZone.Addresses.Contains(unmappedAddressXElement.Value))
										.Remove();

					deliveryZoneXElement.Attribute("Name").SetValue(deliveryZone.Name);
					deliveryZoneXElement.Attribute("Colour").SetValue(deliveryZone.Colour.ToString());
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
		public void RemoveDeliveryZone(string deliveryZoneName)
		{
			if (deliveryZoneName == null)
				throw new ArgumentNullException("deliveryZoneName");
			if (string.IsNullOrEmpty(deliveryZoneName) || string.IsNullOrWhiteSpace(deliveryZoneName))
				throw new ArgumentException("Cannot be empty or whitespace.", "deliveryZoneName");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement deliveryZoneXElement = xmlTransaction.XmlDocument
															  .Root
															  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}DeliveryZone")
															  .FirstOrDefault(deliveryZoneXmlElement => string.Equals(deliveryZoneXmlElement.Attribute("Name").Value, deliveryZoneName, StringComparison.OrdinalIgnoreCase));

				if (deliveryZoneXElement != null)
				{
					xmlTransaction.XmlDocument
								  .Root
								  .Add(deliveryZoneXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address"));
					deliveryZoneXElement.Remove();
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

		private DeliveryZone _GetDeliveryZone(XElement deliveryZoneXmlElement, IApplicationUserProvider applicationUserProvider)
		{
			XAttribute assigneeEMailAddressXAtribute = deliveryZoneXmlElement.Attribute("AssigneeEMailAddress");

			DeliveryZone deliveryZone = new DeliveryZone(deliveryZoneXmlElement.Attribute("Name").Value,
														 Colour.Parse(deliveryZoneXmlElement.Attribute("Colour").Value),
														 deliveryZoneXmlElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Delivery.xsd}Address")
																			   .Select(addressXElement => addressXElement.Value));

			if (assigneeEMailAddressXAtribute != null)
				deliveryZone.Assignee = applicationUserProvider.GetEmployee(assigneeEMailAddressXAtribute.Value);

			return deliveryZone;
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