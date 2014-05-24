﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlOrderRepository
		: IOrderRepository
	{
		public XmlOrderRepository(string xmlDocumentFileName, XmlDocumentProvider xmlDocumentProvider)
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
			_xmlDocumentSchemaSet.Add("http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd", "http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd");
		}

		#region IOrderProvider Members
		public Order GetOrder(IApplicationUserProvider applicationUserProvider, IProductProvider productProvider, int orderNumber, DateTime version)
		{
			if (applicationUserProvider == null)
				throw new ArgumentNullException("applicationUserProvider");
			if (productProvider == null)
				throw new ArgumentNullException("productProvider");

			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, version))
			{
				XElement orderXElement = xmlTransaction.XmlDocument
													   .Root
													   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd}Order")
													   .FirstOrDefault(orderXmlElement => (orderNumber == int.Parse(orderXmlElement.Attribute("OrderNumber").Value)));

				if (orderXElement == null)
					return null;
				else
					return _GetOrder(orderXElement, applicationUserProvider, productProvider);
			}
		}
		#endregion
		#region IOrderRepository Members
		public Order PlaceOrder(OrderDetails orderDetails)
		{
			if (orderDetails == null)
				throw new ArgumentNullException("orderDetails");
			if (!orderDetails.OrderedProducts.Any())
				throw new ArgumentException("There must be at least one ordered product!", "orderDetails");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				int orderNumber = (xmlTransaction.XmlDocument.Root.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd}Order").Count() + 1);
				DateTime now = DateTime.Now;
				XElement orderXmlElement = new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd}Order",
														new XAttribute("OrderNumber", orderNumber),
														new XAttribute("DatePlaced", now.ToString(MvcApplication.DateTimeSerializationFormat)),
														new XAttribute("RecipientEMailAddress", orderDetails.Recipient.EMailAddress),
														new XAttribute("State", OrderState.Pending),
														new XAttribute("DeliveryAddress", orderDetails.DeliveryAddress),
														orderDetails.OrderedProducts.Select(orderedProduct => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd}ProductOrdered",
																														   new XAttribute("Name", orderedProduct.Product.Name),
																														   new XAttribute("Quantity", orderedProduct.Quantity))));

				if (orderDetails.DeliveryAddressLine2 != null)
					orderXmlElement.Add(new XAttribute("DeliveryAddressLine2", orderDetails.DeliveryAddressLine2));

				xmlTransaction.XmlDocument
							  .Root
							  .Add(orderXmlElement);


				try
				{
					xmlTransaction.Commit();
					return new Order(orderNumber, now, orderDetails.Recipient, orderDetails.DeliveryAddress, orderDetails.DeliveryAddressLine2, OrderState.Pending);
				}
				catch (AggregateException xmlExceptions)
				{
					throw new AggregateException(xmlExceptions.InnerExceptions.Select(_TranslateException));
				}
			}
		}
		public IEnumerable<Order> GetOrders(OrderState orderState, IApplicationUserProvider applicationUserProvider, IProductProvider productProvider)
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd}Order")
									 .Where(orderXmlElement => (orderState == (OrderState)Enum.Parse(typeof(OrderState), orderXmlElement.Attribute("State").Value)))
									 .Select(orderXmlElement => _GetOrder(orderXmlElement, applicationUserProvider, productProvider));
		}
		public void UpdateOrders(IEnumerable<Order> orders, OrderState orderState)
		{
			if (orders == null)
				throw new ArgumentNullException("orders");

			IDictionary<int, Order> indexedOrders = new SortedList<int, Order>(orders.Count());
			foreach (Order order in orders)
				indexedOrders.Add(order.OrderNumber, order);

			using (IExclusiveXmlTransaction xmlTransacion = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				foreach (XElement orderXElement in xmlTransacion.XmlDocument
																.Root
																.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd}Order")
																.Where(orderXmlElement => indexedOrders.ContainsKey(int.Parse(orderXmlElement.Attribute("OrderNumber").Value))))
					orderXElement.Attribute("State").SetValue(orderState.ToString());

				xmlTransacion.Commit();
			}
		}

		public IEnumerable<string> GetUsedAddresses()
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName))
				return new SortedSet<string>(xmlTransaction.XmlDocument
														   .Root
														   .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd}Order")
														   .Select(orderXmlElement => orderXmlElement.Attribute("DeliveryAddress").Value));
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

		private Order _GetOrder(XElement orderXElement, IApplicationUserProvider applicationUserProvider, IProductProvider productProvider)
		{
			XAttribute deliveryAddressLine2XmlAttribute = orderXElement.Attribute("DeliveryAddressLine2");
			DateTime datePlaced = DateTime.ParseExact(orderXElement.Attribute("DatePlaced").Value, MvcApplication.DateTimeSerializationFormat, null);

			Order order;
			if (deliveryAddressLine2XmlAttribute != null)
				order = new Order(int.Parse(orderXElement.Attribute("OrderNumber").Value),
								  datePlaced,
								  applicationUserProvider.GetUser(orderXElement.Attribute("RecipientEMailAddress").Value, datePlaced),
								  orderXElement.Attribute("DeliveryAddress").Value,
								  deliveryAddressLine2XmlAttribute.Value,
								  (OrderState)Enum.Parse(typeof(OrderState), orderXElement.Attribute("State").Value));
			else
				order = new Order(int.Parse(orderXElement.Attribute("OrderNumber").Value),
								  datePlaced,
								  applicationUserProvider.GetUser(orderXElement.Attribute("RecipientEMailAddress").Value, datePlaced),
								  orderXElement.Attribute("DeliveryAddress").Value,
								  orderState: (OrderState)Enum.Parse(typeof(OrderState), orderXElement.Attribute("State").Value));

			foreach (XElement orderedProductXElement in orderXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Order.xsd}ProductOrdered"))
				order.OrderedProducts.Add(new OrderedProduct(productProvider.GetProduct(orderedProductXElement.Attribute("Name").Value, datePlaced),
															 int.Parse(orderedProductXElement.Attribute("Quantity").Value)));

			return order;
		}

		private Exception _TranslateException(Exception exception)
		{
			return exception;
		}

		private string _xmlDocumentFileName;
		private XmlDocumentProvider _xmlDocumentProvider;
		private readonly XmlSchemaSet _xmlDocumentSchemaSet;
	}
}