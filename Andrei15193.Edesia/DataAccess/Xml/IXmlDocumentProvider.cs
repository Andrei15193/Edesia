using System;
using System.Xml.Linq;
using System.Xml.Schema;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public interface IXmlDocumentProvider
	{
		event EventHandler<XmlDocumentEventArgs> LoadingXmlDocument;
		event EventHandler<XmlDocumentEventArgs> LoadedXmlDocument;
		event EventHandler<XmlDocumentEventArgs> SavingXmlDocument;
		event EventHandler<XmlDocumentEventArgs> SavedXmlDocument;
		event ValidationEventHandler XmlDocumentError;
		bool ValidateSourceDocument
		{
			get;
			set;
		}
		XmlSchemaSet XmlDocumentSchemaSet
		{
			get;
		}
		XDocument LoadXmlDocument(string xmlDocumentFileName);
		void SaveXmlDocument(XDocument xDocument, string xmlDocumentFileName);
	}
}