using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Xml.Validation;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	/// <summary>
	/// Represents an abstract XmlDocumentProvider. Such providers are responsible with retrieving and saving
	/// valid XML documents. A source XML document can be considered to be valid however every time a save
	/// is attempted the provided XML document is validated.
	/// </summary>
	/// <remarks>
	/// This is a thread safe class.
	/// </remarks>
	public abstract class XmlDocumentProvider
	{
		/// <summary>
		/// Loads a XML document, optionally validating it, having the provided name.
		/// This method is thread safe.
		/// </summary>
		/// <param name="xmlDocumentName">The name of the XML document to load.</param>
		/// <param name="xmlSchemaSet">The XML schema to use to validate the loaded document.</param>
		/// <returns>Returns a XML document.</returns>
		/// <exception cref="System.AggregateException">
		/// Thrown when xmlSchemaSet is provided and there is at least one XML schema inconsistency.
		/// </exception>
		[Obsolete("Use BeginXmlTransaction method to read/write to an XML document.")]
		public XDocument LoadXmlDocument(string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null)
		{
			if (xmlDocumentName == null)
				throw new ArgumentNullException("xmlDocumentName");
			if (string.IsNullOrEmpty(xmlDocumentName) || string.IsNullOrWhiteSpace(xmlDocumentName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentName");

			XDocument xmlDocument = OnLoadXmlDocument(xmlDocumentName);
			if (xmlDocument == null)
				throw new InvalidOperationException();

			if (xmlSchemaSet != null)
				Validate(xmlDocument, xmlSchemaSet);

			return xmlDocument;
		}
		/// <summary>
		/// Saves the XML document, optionally validating it, using the given name.
		/// This method is thread safe.
		/// </summary>
		/// <param name="xmlDocument">The XML document to save.</param>
		/// <param name="xmlDocumentName">The name with which to save the XML document.</param>
		/// <param name="xmlSchemaSet">The XML schema set to use when validating the document before actual save.</param>
		/// <exception cref="System.AggregateException">
		/// Thrown when xmlSchemaSet is provided and there is at least one XML schema inconsistency.
		/// </exception>
		[Obsolete("Use BeginXmlTransaction method to read/write to an XML document.")]
		public void SaveXmlDocument(XDocument xmlDocument, string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null)
		{
			if (xmlDocument == null)
				throw new ArgumentNullException("xmlDocument");
			if (xmlDocumentName == null)
				throw new ArgumentNullException("xmlDocumentName");
			if (string.IsNullOrEmpty(xmlDocumentName) || string.IsNullOrWhiteSpace(xmlDocumentName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentName");

			if (xmlSchemaSet != null)
				Validate(xmlDocument, xmlSchemaSet);
			OnSaveXmlDocument(xmlDocument, xmlDocumentName);
		}
		/// <summary>
		/// Returns a collection XML Schema Exception interpreters.
		/// </summary>
		public ICollection<IXmlSchemaExceptionInterpreter<XmlSchemaException>> XmlSchemaExceptionInterpreters
		{
			get
			{
				return _xmlSchemaExceptionInterpreters;
			}
		}

		/// <summary>
		/// Loads an XML document into a transaction, optionally validating it, having the provided name.
		/// This method is thread safe.
		/// </summary>
		/// <param name="xmlDocumentName">The name of the XML document to load.</param>
		/// <param name="xmlSchemaSet">The XML schema to use to validate the loaded document.</param>
		/// <returns>Returns a XML document.</returns>
		/// <exception cref="System.AggregateException">
		/// Thrown when xmlSchemaSet is provided and there is at least one XML schema inconsistency.
		/// </exception>
		public abstract IXmlTransaction BeginXmlTransaction(string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null);

		/// <summary>
		/// When implemented in a derived class it loads a XML document with the given XML document name.
		/// This methos is thread safe.
		/// </summary>
		/// <param name="xmlDocumentName">The XML document name to load.</param>
		/// <returns>Returns an XML document.</returns>
		[Obsolete("Use BeginXmlTransaction method to read/write to an XML document.")]
		protected abstract XDocument OnLoadXmlDocument(string xmlDocumentName);
		/// <summary>
		/// When implemented in a derived class it saves the given XML document using the given XML document name.
		/// This method is thread safe.
		/// </summary>
		/// <param name="xmlDocument">The XML document to save.</param>
		/// <param name="xmlDocumentName">The name with which to save the XML document.</param>
		[Obsolete("Use BeginXmlTransaction method to read/write to an XML document.")]
		protected abstract void OnSaveXmlDocument(XDocument xmlDocument, string xmlDocumentName);

		protected void Validate(XDocument xmlDocument, XmlSchemaSet xmlSchemaSet)
		{
			ICollection<Exception> xmlSchemaExceptions = new LinkedList<Exception>();

			xmlDocument.Validate(xmlSchemaSet, (sender, e) =>
				{
					XmlSchemaException xmlSchemaException = _xmlSchemaExceptionInterpreters.Select(interpreter => interpreter.Interpret(e.Exception))
																					  .Where(exception => exception != null)
																					  .FirstOrDefault();

					if (xmlSchemaException != null)
						xmlSchemaExceptions.Add(xmlSchemaException);
				});

			if (xmlSchemaExceptions.Any())
				throw new AggregateException(xmlSchemaExceptions);
		}
		private readonly ICollection<IXmlSchemaExceptionInterpreter<XmlSchemaException>> _xmlSchemaExceptionInterpreters = new List<IXmlSchemaExceptionInterpreter<XmlSchemaException>>();
	}
}