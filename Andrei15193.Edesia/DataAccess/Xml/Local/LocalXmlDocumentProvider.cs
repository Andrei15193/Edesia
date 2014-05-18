using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;
namespace Andrei15193.Edesia.DataAccess.Xml.Local
{
	public sealed class LocalXmlDocumentProvider
		: XmlDocumentProvider
	{
		public override IExclusiveXmlTransaction BeginExclusiveTransaction(string xmlDocumentName, DateTime version, XmlSchemaSet xmlSchemaSet = null)
		{
			if (xmlDocumentName == null)
				throw new ArgumentNullException("xmlDocumentName");
			if (string.IsNullOrWhiteSpace(xmlDocumentName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentName");

			lock (_documentLocks)
			{
				ReaderWriterLockSlim versionFileLock;

				if (!_documentLocks.TryGetValue(xmlDocumentName, out versionFileLock))
				{
					versionFileLock = new ReaderWriterLockSlim();
					_documentLocks.Add(xmlDocumentName, versionFileLock);
				}

				try
				{
					versionFileLock.EnterWriteLock();
					XDocument versionXmlDocument;
					string versionXmlDocumentFilePath = Combine(Directory.CreateDirectory(Combine(DirectoryPath, "." + xmlDocumentName)).FullName, "versions.xml");

					if (File.Exists(versionXmlDocumentFilePath))
						versionXmlDocument = XDocument.Load(versionXmlDocumentFilePath);
					else
					{
						FileInfo xmlDocumentFileInfo = new FileInfo(Combine(DirectoryPath, xmlDocumentName));

						versionXmlDocument = new XDocument(new XElement("File",
																		new XElement("Version",
																					 new XAttribute("FileName", xmlDocumentFileInfo.CopyTo(Combine(DirectoryPath, "." + xmlDocumentName, xmlDocumentFileInfo.CreationTime.ToString("yyyy_MM_dd__HH_mm_ss_fffffff'.xml'"))).FullName),
																					 new XAttribute("BeginDate", xmlDocumentFileInfo.CreationTime.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz")))));
						versionXmlDocument.Save(versionXmlDocumentFilePath);
					}

					XElement selectedVersionXElement = versionXmlDocument.Root.Elements("Version").TakeWhile(versionXmlElement => version >= DateTime.ParseExact(versionXmlElement.Attribute("BeginDate").Value, "yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz", null)).LastOrDefault();
					if (selectedVersionXElement == null)
						throw new ArgumentException("The specified version is before any known version of the file!", "version");

					XDocument xmlDocument = XDocument.Load(selectedVersionXElement.Attribute("FileName").Value);

					if (xmlSchemaSet != null)
						Validate(xmlDocument, xmlSchemaSet);

					return new XmlTransaction(xmlDocument,
											  () =>
											  {
												  DateTime now = DateTime.Now;
												  string xmlDocumentFileName = Combine(DirectoryPath, "." + xmlDocumentName, now.ToString("yyyy_MM_dd__HH_mm_ss_fffffff'.xml'"));

												  if (selectedVersionXElement.Attribute("EndDate") == null)
													  selectedVersionXElement.Add(new XAttribute("EndDate", now.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz")));
												  else
													  versionXmlDocument.Root.Elements("Version").Last().Add(new XAttribute("EndDate", now.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz")));
												  versionXmlDocument.Root
																	.Add(new XElement("Version",
																					  new XAttribute("FileName", xmlDocumentFileName),
																					  new XAttribute("BeginDate", now.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz"))));

												  if (xmlSchemaSet != null)
													  Validate(xmlDocument, xmlSchemaSet);

												  xmlDocument.Save(xmlDocumentFileName);
												  versionXmlDocument.Save(versionXmlDocumentFilePath);
											  },
											  () =>
											  {
												  versionFileLock.ExitWriteLock();
											  });
				}
				catch
				{
					versionFileLock.ExitWriteLock();
					throw;
				}
			}
		}
		public override ISharedXmlTransaction BeginSharedTransaction(string xmlDocumentName, DateTime version, XmlSchemaSet xmlSchemaSet = null)
		{
			if (xmlDocumentName == null)
				throw new ArgumentNullException("xmlDocumentName");
			if (string.IsNullOrWhiteSpace(xmlDocumentName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentName");

			lock (_documentLocks)
			{
				ReaderWriterLockSlim versionFileLock;

				if (!_documentLocks.TryGetValue(xmlDocumentName, out versionFileLock))
				{
					versionFileLock = new ReaderWriterLockSlim();
					_documentLocks.Add(xmlDocumentName, versionFileLock);
				}

				try
				{
					versionFileLock.EnterReadLock();
					XDocument versionXmlDocument;
					string versionXmlDocumentFilePath = Combine(Directory.CreateDirectory(Combine(DirectoryPath, "." + xmlDocumentName)).FullName, "versions.xml");

					if (File.Exists(versionXmlDocumentFilePath))
						versionXmlDocument = XDocument.Load(versionXmlDocumentFilePath);
					else
					{
						FileInfo xmlDocumentFileInfo = new FileInfo(Combine(DirectoryPath, xmlDocumentName));

						versionXmlDocument = new XDocument(new XElement("File",
																		new XElement("Version",
																					 new XAttribute("FileName", xmlDocumentFileInfo.CopyTo(Combine(DirectoryPath, "." + xmlDocumentName, xmlDocumentFileInfo.CreationTime.ToString("yyyy_MM_dd__HH_mm_ss_fffffff'.xml'"))).FullName),
																					 new XAttribute("BeginDate", xmlDocumentFileInfo.CreationTime.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz")))));
						versionXmlDocument.Save(versionXmlDocumentFilePath);
					}

					XElement selectedVersionXElement = versionXmlDocument.Root.Elements("Version").LastOrDefault(versionXmlElement => version >= DateTime.ParseExact(versionXmlElement.Attribute("BeginDate").Value, "yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz", null));
					if (selectedVersionXElement == null)
						throw new ArgumentException("The specified version is before any known version of the file!", "version");

					XDocument xmlDocument = XDocument.Load(selectedVersionXElement.Attribute("FileName").Value);

					if (xmlSchemaSet != null)
						Validate(xmlDocument, xmlSchemaSet);

					return new XmlTransaction(xmlDocument,
											  disposeAction: () =>
											  {
												  versionFileLock.ExitReadLock();
											  });
				}
				catch
				{
					versionFileLock.ExitReadLock();
					throw;
				}
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
			//using (XmlWriter xmlWriter = XmlWriter.Create(xmlDocumentFileName, _xmlWriterSettings))
				//xDocument.Save(xmlWriter);
		}

		private void _SaveXmlDocument(XDocument xmlDocument, string xmlDocumentName, XmlSchemaSet xmlSchemaSet)
		{
			if (xmlSchemaSet != null)
				Validate(xmlDocument, xmlSchemaSet);

			xmlDocument.Save(xmlDocumentName);
		}

		private readonly IDictionary<string, ReaderWriterLockSlim> _documentLocks = new SortedList<string, ReaderWriterLockSlim>();
	}
}