using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
namespace Andrei15193.Edesia.DataAccess.Xml.Local
{
	public class LocalXmlDocumentProvider
		: IXmlDocumentProvider
	{
		public LocalXmlDocumentProvider()
		{
			ValidateSourceDocument = true;
		}

		#region IXmlDocumentProvider Members
		public event EventHandler<XmlDocumentEventArgs> LoadingXmlDocument;
		public event EventHandler<XmlDocumentEventArgs> LoadedXmlDocument;
		public event EventHandler<XmlDocumentEventArgs> SavingXmlDocument;
		public event EventHandler<XmlDocumentEventArgs> SavedXmlDocument;
		public event ValidationEventHandler XmlDocumentError;
		public bool ValidateSourceDocument
		{
			get;
			set;
		}
		public XmlSchemaSet XmlDocumentSchemaSet
		{
			get
			{
				return _xmlSchemaSet;
			}
		}
		public XDocument LoadXmlDocument(string xmlDocumentFileName)
		{
			_RaiseEvent(LoadingXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName, null));

			XDocument xDocument = XDocument.Load(xmlDocumentFileName);

			if (ValidateSourceDocument)
				xDocument.Validate(XmlDocumentSchemaSet, XmlDocumentError);

			_RaiseEvent(LoadedXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName, null));

			return xDocument;
		}
		public void SaveXmlDocument(XDocument xDocument, string xmlDocumentFileName)
		{
			_RaiseEvent(SavingXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName, xDocument));

			xDocument.Validate(XmlDocumentSchemaSet, XmlDocumentError);
			using (XmlWriter xmlWriter = XmlWriter.Create(xmlDocumentFileName, _xmlWriterSettings))
				xDocument.Save(xmlWriter);

			_RaiseEvent(SavedXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName, xDocument));
		}
		#endregion

		private void _RaiseEvent(EventHandler eventHandler, EventArgs eventArgs)
		{
			if (eventHandler != null)
				eventHandler(this, eventArgs);
		}
		private void _RaiseEvent<T>(EventHandler<T> eventHandler, T eventArgs)
			where T : EventArgs
		{
			if (eventHandler != null)
				eventHandler(this, eventArgs);
		}

		private readonly XmlSchemaSet _xmlSchemaSet = new XmlSchemaSet();
		private static readonly XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings
			{
				ConformanceLevel = ConformanceLevel.Document,
				Encoding = Encoding.UTF8,
				Indent = true,
				IndentChars = "\t",
				NamespaceHandling = NamespaceHandling.OmitDuplicates,
				NewLineChars = Environment.NewLine,
				OmitXmlDeclaration = false,
			};
	}
}