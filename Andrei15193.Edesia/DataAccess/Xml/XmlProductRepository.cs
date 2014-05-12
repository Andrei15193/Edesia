using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlProductRepository
		: IProductRepository
	{
		public XmlProductRepository(string xmlDocumentFileName, XmlDocumentProvider xmlDocumentProvider)
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
			_xmlDocumentSchemaSet.Add("http://storage.andrei15193.ro/public/schemas/Edesia/Consumer.xsd", "http://storage.andrei15193.ro/public/schemas/Edesia/Consumer.xsd");
		}

		#region IProductRepository Members
		public IEnumerable<Product> GetProducts()
		{
			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Consumer.xsd}Product")
									 .Where(productXElement => productXElement.Attribute("DateRemoved") == null)
									 .Select(_GetProduct);
		}

		public void AddProduct(Product product)
		{
			if (product == null)
				throw new ArgumentNullException("product");

			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				if (xmlTransaction.XmlDocument
								  .Root
								  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Consumer.xsd}Product")
								  .Any(productXElement => string.Equals(productXElement.Attribute("Name").Value, product.Name, StringComparison.OrdinalIgnoreCase)
														  && (productXElement.Attribute("DateRemoved") == null || DateTime.ParseExact(productXElement.Attribute("DateRemoved").Value, MvcApplication.DateTimeSerializationFormat, null) >= product.DateAdded)
														  && (!product.DateRemoved.HasValue || product.DateRemoved.Value >= DateTime.ParseExact(productXElement.Attribute("DateAdded").Value, MvcApplication.DateTimeSerializationFormat, null))))
					throw new AggregateException(new UniqueProductException(product.Name));

				if (!product.DateRemoved.HasValue)
					xmlTransaction.XmlDocument
								  .Root
								  .Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Consumer.xsd}Product",
													new XAttribute("Name", product.Name),
													new XAttribute("Price", product.Price),
													new XAttribute("DateAdded", product.DateAdded.ToString(MvcApplication.DateTimeSerializationFormat))));
				else
					xmlTransaction.XmlDocument
								  .Root
								  .Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Consumer.xsd}Product",
													new XAttribute("Name", product.Name),
													new XAttribute("Price", product.Price),
													new XAttribute("DateAdded", product.DateAdded.ToString(MvcApplication.DateTimeSerializationFormat)),
													new XAttribute("DateRemoved", product.DateRemoved.Value.ToString(MvcApplication.DateTimeSerializationFormat))));

				xmlTransaction.Commit();
			}
		}
		public void RemoveProduct(string productName)
		{
			using (IXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginXmlTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement productXElement = xmlTransaction.XmlDocument
														 .Root
														 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Consumer.xsd}Product")
														 .FirstOrDefault(productXmlElement => string.Equals(productXmlElement.Attribute("Name").Value, productName, StringComparison.OrdinalIgnoreCase)
																							  && productXmlElement.Attribute("DateRemoved") == null);

				if (productXElement != null)
				{
					productXElement.Add(new XAttribute("DateRemoved", DateTime.Now.ToString(MvcApplication.DateTimeSerializationFormat)));
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

		private Product _GetProduct(XElement productXElement)
		{
			XAttribute dateRemovedXAttribute = productXElement.Attribute("DateRemoved");

			if (dateRemovedXAttribute == null)
				return new Product(productXElement.Attribute("Name").Value,
								   double.Parse(productXElement.Attribute("Price").Value),
								   DateTime.ParseExact(productXElement.Attribute("DateAdded").Value, MvcApplication.DateTimeSerializationFormat, null));
			else
				return new Product(productXElement.Attribute("Name").Value,
								   double.Parse(productXElement.Attribute("Price").Value),
								   DateTime.ParseExact(productXElement.Attribute("DateAdded").Value, MvcApplication.DateTimeSerializationFormat, null),
								   DateTime.ParseExact(dateRemovedXAttribute.Value, MvcApplication.DateTimeSerializationFormat, null));
		}

		private string _xmlDocumentFileName;
		private XmlDocumentProvider _xmlDocumentProvider;
		private readonly XmlSchemaSet _xmlDocumentSchemaSet;
	}
}