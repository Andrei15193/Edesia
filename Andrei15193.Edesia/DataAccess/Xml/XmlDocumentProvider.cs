using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;
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
		public XDocument LoadXmlDocument(string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null)
		{
			if (xmlDocumentName == null)
				throw new ArgumentNullException("xmlDOcumentFileName");
			if (string.IsNullOrEmpty(xmlDocumentName) || string.IsNullOrWhiteSpace(xmlDocumentName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentFileName");

			XDocument xmlDocument = OnLoadXmlDocument(xmlDocumentName);
			if (xmlDocument == null)
				throw new InvalidOperationException();

			if (xmlSchemaSet != null)
				xmlDocument.Validate(xmlSchemaSet, _ProperSchemaValidation);

			return xmlDocument;
		}
		/// <summary>
		/// Saves the XML document, optionally validating it, using the given name.
		/// This method is thread safe.
		/// </summary>
		/// <param name="xmlDocument">The XML document to save.</param>
		/// <param name="xmlDocumentName">The name with which to save the XML document.</param>
		/// <param name="xmlSchemaSet">The XML schema set to use when validating the document before actual save.</param>
		public void SaveXmlDocument(XDocument xmlDocument, string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null)
		{
			if (xmlDocument == null)
				throw new ArgumentNullException("xmlDocument");
			if (xmlDocumentName == null)
				throw new ArgumentNullException("xmlDOcumentFileName");
			if (string.IsNullOrEmpty(xmlDocumentName) || string.IsNullOrWhiteSpace(xmlDocumentName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentFileName");

			if (xmlSchemaSet != null)
				xmlDocument.Validate(xmlSchemaSet, _ProperSchemaValidation);
			OnSaveXmlDocument(xmlDocument, xmlDocumentName);
		}

		/// <summary>
		/// When implemented in a derived class it loads a XML document with the given XML document name.
		/// This methos is thread safe.
		/// </summary>
		/// <param name="xmlDocumentName">The XML document name to load.</param>
		/// <returns>Returns an XML document.</returns>
		protected abstract XDocument OnLoadXmlDocument(string xmlDocumentName);
		/// <summary>
		/// When implemented in a derived class it saves the given XML document using the given XML document name.
		/// This method is thread safe.
		/// </summary>
		/// <param name="xmlDocument">The XML document to save.</param>
		/// <param name="xmlDocumentName">The name with which to save the XML document.</param>
		protected abstract void OnSaveXmlDocument(XDocument xmlDocument, string xmlDocumentName);

		private static void _ProperSchemaValidation(object sender, ValidationEventArgs e)
		{
			switch (e.Exception.HResult)
			{
				case -2146231999:
					Match errorMessageMatch = Regex.Match(e.Exception.Message, "There is a duplicate key sequence '(.*)' for the '(.*)' key or unique identity constraint.");
					throw new UnsatisfiedUniqueConstraintException(errorMessageMatch.Groups[1].Value, errorMessageMatch.Groups[2].Value);
				default:
					throw e.Exception;
			}
		}
	}
}