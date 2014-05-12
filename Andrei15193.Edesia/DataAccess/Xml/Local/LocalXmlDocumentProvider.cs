using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
namespace Andrei15193.Edesia.DataAccess.Xml.Local
{
	public sealed class LocalXmlDocumentProvider
		: XmlDocumentProvider
	{
		public override IXmlTransaction BeginXmlTransaction(string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null)
		{
			if (xmlDocumentName == null)
				throw new ArgumentNullException("xmlDocumentName");
			if (string.IsNullOrWhiteSpace(xmlDocumentName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentName");

			lock (_documentSpinLocksLock)
			{
				SpinLock spinLock;
				XDocument xmlDocument = XDocument.Load(xmlDocumentName);

				if (xmlSchemaSet != null)
					Validate(xmlDocument, xmlSchemaSet);

				if (!_documentSpinLocks.TryGetValue(xmlDocumentName, out spinLock))
				{
					spinLock = new SpinLock(enableThreadOwnerTracking: false);
					_documentSpinLocks.Add(xmlDocumentName, spinLock);
				}

				bool lockTaken = false;
				spinLock.Enter(ref lockTaken);

				return new XmlTransaction(xmlDocument, () => _SaveXmlDocument(xmlDocument, xmlDocumentName, xmlSchemaSet), () => spinLock.Exit());
			}
		}

		[Obsolete]
		protected override XDocument OnLoadXmlDocument(string xmlDocumentFileName)
		{
			return XDocument.Load(xmlDocumentFileName);
		}
		[Obsolete]
		protected override void OnSaveXmlDocument(XDocument xDocument, string xmlDocumentFileName)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(xmlDocumentFileName, _xmlWriterSettings))
				xDocument.Save(xmlWriter);
		}

		private void _SaveXmlDocument(XDocument xmlDocument, string xmlDocumentName, XmlSchemaSet xmlSchemaSet)
		{
			if (xmlSchemaSet != null)
				Validate(xmlDocument, xmlSchemaSet);

			xmlDocument.Save(xmlDocumentName);
		}

		private readonly object _documentSpinLocksLock = new object();
		private readonly IDictionary<string, SpinLock> _documentSpinLocks = new SortedList<string, SpinLock>(StringComparer.OrdinalIgnoreCase);
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