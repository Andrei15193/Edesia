using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace Andrei15193.Edesia.DataAccess.Xml.Local
{
	public class LocalXmlDocumentProvider
		: XmlDocumentProvider
	{
		protected override XDocument OnLoadXmlDocument(string xmlDocumentFileName)
		{
			return XDocument.Load(xmlDocumentFileName);
		}
		protected override void OnSaveXmlDocument(XDocument xDocument, string xmlDocumentFileName)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(xmlDocumentFileName, _xmlWriterSettings))
				xDocument.Save(xmlWriter);
		}

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