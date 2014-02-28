using System;
using System.Xml.Linq;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlDocumentEventArgs
		: EventArgs
	{
		public XmlDocumentEventArgs(string documentName, XDocument xmlDocument = null)
		{
			if (documentName == null)
				throw new ArgumentNullException("documentName");
			if (string.IsNullOrEmpty(documentName) || string.IsNullOrWhiteSpace(documentName))
				throw new ArgumentException("Cannot be empty or whitespace", "documentName");
			_documentName = documentName;
			_xmlDocument = xmlDocument;
		}

		public string DocumentName
		{
			get
			{
				return _documentName;
			}
		}
		public XDocument XmlDocument
		{
			get
			{
				return _xmlDocument;
			}
		}

		private readonly string _documentName;
		private readonly XDocument _xmlDocument;
	}
}