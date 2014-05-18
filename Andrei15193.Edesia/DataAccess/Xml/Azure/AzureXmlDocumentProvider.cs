using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
namespace Andrei15193.Edesia.DataAccess.Xml.Azure
{
	public sealed class AzureXmlDocumentProvider
		: XmlDocumentProvider
	{
		public AzureXmlDocumentProvider(string connectionStringCloudSettingName)
		{
			if (connectionStringCloudSettingName == null)
				throw new ArgumentNullException("connectionStringCloudSettingName");
			if (string.IsNullOrEmpty(connectionStringCloudSettingName) || string.IsNullOrWhiteSpace(connectionStringCloudSettingName))
				throw new ArgumentException("Cannot be empty or whitespace!", "connectionStringCloudSettingName");

			_connectionStringCloudSettingName = connectionStringCloudSettingName;
		}

		public override IExclusiveXmlTransaction BeginExclusiveTransaction(string xmlDocumentName, DateTime version, XmlSchemaSet xmlSchemaSet = null)
		{
			if (xmlDocumentName == null)
				throw new ArgumentNullException("xmlDocumentName");
			if (string.IsNullOrWhiteSpace(xmlDocumentName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentName");

			lock (_documentLocks)
			{
				ReaderWriterLockSlim versionFileLock;
				CloudBlobContainer blobContainer = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(_connectionStringCloudSettingName))
																	  .CreateCloudBlobClient()
																	  .GetContainerReference("andrei15193");

				if (!_documentLocks.TryGetValue(xmlDocumentName, out versionFileLock))
				{
					versionFileLock = new ReaderWriterLockSlim();
					_documentLocks.Add(xmlDocumentName, versionFileLock);
				}

				try
				{
					versionFileLock.EnterWriteLock();
					XDocument versionXmlDocument;
					CloudBlockBlob versionXmlDocumentBlob = blobContainer.GetBlockBlobReference(Combine(DirectoryPath, xmlDocumentName, "Versions.xml"));

					if (versionXmlDocumentBlob.Exists())
						using (Stream readStream = versionXmlDocumentBlob.OpenRead())
							versionXmlDocument = XDocument.Load(readStream);
					else
					{
						CloudBlockBlob originalXmlDocumentBlob = blobContainer.GetBlockBlobReference(Combine(DirectoryPath, xmlDocumentName));
						CloudBlockBlob xmlDocumentBlob = blobContainer.GetBlockBlobReference(Combine(DirectoryPath, xmlDocumentName, (originalXmlDocumentBlob.Properties.LastModified ?? DateTimeOffset.MinValue).DateTime.ToString("yyyy_MM_dd__HH_mm_ss_fffffff'.xml'")));

						versionXmlDocument = new XDocument(new XElement("File",
																		new XElement("Version",
																					 new XAttribute("FileName", xmlDocumentBlob.Name),
																					 new XAttribute("BeginDate", (originalXmlDocumentBlob.Properties.LastModified ?? DateTimeOffset.MinValue).DateTime.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz")))));

						xmlDocumentBlob.UploadText(originalXmlDocumentBlob.DownloadText());
						versionXmlDocumentBlob.UploadText(versionXmlDocument.ToString());
					}

					XElement selectedVersionXElement = versionXmlDocument.Root.Elements("Version").TakeWhile(versionXmlElement => version >= DateTime.ParseExact(versionXmlElement.Attribute("BeginDate").Value, "yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz", null)).LastOrDefault();
					if (selectedVersionXElement == null)
						throw new ArgumentException("The specified version is before any known version of the file!", "version");

					XDocument xmlDocument;
					using (Stream readStream = blobContainer.GetBlockBlobReference(selectedVersionXElement.Attribute("FileName").Value).OpenRead())
						xmlDocument = XDocument.Load(readStream);

					if (xmlSchemaSet != null)
						Validate(xmlDocument, xmlSchemaSet);

					return new XmlTransaction(xmlDocument,
											  () =>
											  {
												  DateTime now = DateTime.Now;
												  CloudBlockBlob xmlDocumentBlob = blobContainer.GetBlockBlobReference(Combine(DirectoryPath, xmlDocumentName, now.ToString("yyyy_MM_dd__HH_mm_ss_fffffff'.xml'")));

												  if (selectedVersionXElement.Attribute("EndDate") == null)
													  selectedVersionXElement.Add(new XAttribute("EndDate", now.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz")));
												  else
													  versionXmlDocument.Root.Elements("Version").Last().Add(new XAttribute("EndDate", now.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz")));
												  versionXmlDocument.Root
																	.Add(new XElement("Version",
																					  new XAttribute("FileName", xmlDocumentBlob.Name),
																					  new XAttribute("BeginDate", now.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz"))));

												  if (xmlSchemaSet != null)
													  Validate(xmlDocument, xmlSchemaSet);

												  xmlDocumentBlob.UploadText(xmlDocument.ToString());
												  versionXmlDocumentBlob.UploadText(versionXmlDocument.ToString());
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
				CloudBlobContainer blobContainer = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(_connectionStringCloudSettingName))
																	  .CreateCloudBlobClient()
																	  .GetContainerReference("andrei15193");

				if (!_documentLocks.TryGetValue(xmlDocumentName, out versionFileLock))
				{
					versionFileLock = new ReaderWriterLockSlim();
					_documentLocks.Add(xmlDocumentName, versionFileLock);
				}

				try
				{
					versionFileLock.EnterReadLock();
					XDocument versionXmlDocument;
					CloudBlockBlob versionXmlDocumentBlob = blobContainer.GetBlockBlobReference(Combine(DirectoryPath, xmlDocumentName, "Versions.xml"));
					
					if (versionXmlDocumentBlob.Exists())
						using (Stream readStream = versionXmlDocumentBlob.OpenRead())
							versionXmlDocument = XDocument.Load(readStream);
					else
					{
						CloudBlockBlob originalXmlDocumentBlob = blobContainer.GetBlockBlobReference(Combine(DirectoryPath, xmlDocumentName));
						CloudBlockBlob xmlDocumentBlob = blobContainer.GetBlockBlobReference(Combine(DirectoryPath, xmlDocumentName, (originalXmlDocumentBlob.Properties.LastModified ?? DateTimeOffset.MinValue).DateTime.ToString("yyyy_MM_dd__HH_mm_ss_fffffff'.xml'")));

						versionXmlDocument = new XDocument(new XElement("File",
																		new XElement("Version",
																					 new XAttribute("FileName", xmlDocumentBlob.Name),
																					 new XAttribute("BeginDate", (originalXmlDocumentBlob.Properties.LastModified ?? DateTimeOffset.MinValue).DateTime.ToString("yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz")))));

						xmlDocumentBlob.UploadText(originalXmlDocumentBlob.DownloadText());
						versionXmlDocumentBlob.UploadText(versionXmlDocument.ToString());
					}

					XElement selectedVersionXElement = versionXmlDocument.Root.Elements("Version").TakeWhile(versionXmlElement => version >= DateTime.ParseExact(versionXmlElement.Attribute("BeginDate").Value, "yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz", null)).LastOrDefault();
					if (selectedVersionXElement == null)
						throw new ArgumentException("The specified version is before any known version of the file!", "version");

					XDocument xmlDocument;
					using (Stream readStream = blobContainer.GetBlockBlobReference(selectedVersionXElement.Attribute("FileName").Value).OpenRead())
						xmlDocument = XDocument.Load(readStream);

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

		private Action _SaveXmlDocument(CloudBlockBlob dataBlob, XDocument xmlDocument, XmlSchemaSet xmlSchemaSet)
		{
			return () =>
			{
				if (xmlSchemaSet != null)
					Validate(xmlDocument, xmlSchemaSet);

				dataBlob.UploadText(xmlDocument.ToString());
			};
		}
		private Action _ReleaseSpinLock(SpinLock xmlDocumentSpinLock)
		{
			return () => xmlDocumentSpinLock.Exit();
		}

		private readonly object _documentSpinLocksLock = new object();
		private readonly IDictionary<string, SpinLock> _documentSpinLocks = new SortedList<string, SpinLock>(StringComparer.OrdinalIgnoreCase);
		
		private readonly IDictionary<string, ReaderWriterLockSlim> _documentLocks = new SortedList<string, ReaderWriterLockSlim>();

		private readonly string _connectionStringCloudSettingName;
	}
}