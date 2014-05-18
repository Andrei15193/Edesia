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
			_xmlDocumentSchemaSet.Add("http://storage.andrei15193.ro/public/schemas/Edesia/Product.xsd", "http://storage.andrei15193.ro/public/schemas/Edesia/Product.xsd");
		}

		#region IProductRepository Members
		public IEnumerable<Product> GetProducts()
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Product.xsd}Product")
									 .Select(_GetProduct);
		}

		public void AddProduct(Product product)
		{
			if (product == null)
				throw new ArgumentNullException("product");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				xmlTransaction.XmlDocument
							  .Root
							  .Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/Product.xsd}Product",
												new XAttribute("Name", product.Name),
												new XAttribute("Price", product.Price)));
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
		public void RemoveProduct(string productName)
		{
			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement productXElement = xmlTransaction.XmlDocument
														 .Root
														 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Product.xsd}Product")
														 .FirstOrDefault(productXmlElement => string.Equals(productXmlElement.Attribute("Name").Value, productName, StringComparison.OrdinalIgnoreCase));

				if (productXElement != null)
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
		#endregion
		#region IProductProvider Members
		public Product GetProduct(string name, DateTime version)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or whitespace!", "name");

			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, version))
			{
				XElement productXElement = xmlTransaction.XmlDocument
														 .Root
														 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/Product.xsd}Product")
														 .FirstOrDefault(productXmlElement => string.Equals(productXmlElement.Attribute("Name").Value, name, StringComparison.Ordinal));

				if (productXElement == null)
					return null;

				return _GetProduct(productXElement);
			}
		}
		public Product GetProduct(string name)
		{
			return GetProduct(name, DateTime.Now);
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
			return new Product(productXElement.Attribute("Name").Value,
							   double.Parse(productXElement.Attribute("Price").Value));
		}
		private Exception _TranslateException(Exception exception)
		{
			XmlUniqueConstraintException xmlUniqueConstraintException = exception as XmlUniqueConstraintException;

			if (xmlUniqueConstraintException != null && string.Equals("http://storage.andrei15193.ro/public/schemas/Edesia/Product.xsd:UniqueProductNames", xmlUniqueConstraintException.ConstraintName, StringComparison.Ordinal))
					return new UniqueProductException(xmlUniqueConstraintException.ConflictingValue, xmlUniqueConstraintException);

			return exception;
		}

		private string _xmlDocumentFileName;
		private XmlDocumentProvider _xmlDocumentProvider;
		private readonly XmlSchemaSet _xmlDocumentSchemaSet;
	}
}