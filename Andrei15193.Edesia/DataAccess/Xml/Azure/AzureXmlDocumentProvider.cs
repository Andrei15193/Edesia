using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
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

		[Obsolete]
		protected override XDocument OnLoadXmlDocument(string xmlDocumentBlobName)
		{
			CachedXmlDocument cachedDocument;
			CloudBlockBlob dataBlob = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(_connectionStringCloudSettingName))
														 .CreateCloudBlobClient()
														 .GetContainerReference("andrei15193")
														 .GetBlockBlobReference(xmlDocumentBlobName);

			try
			{
				_cacheLock.EnterReadLock();

				if (_cachedDocuments.TryGetValue(xmlDocumentBlobName, out cachedDocument)
					&& cachedDocument.LastModifiedTime == dataBlob.Properties.LastModified)
					return cachedDocument.XmlDocument;
				Interlocked.Increment(ref _readersWaitingForUpdate);
			}
			finally
			{
				_cacheLock.ExitReadLock();
			}

			if (_getDataLock.TryEnterWriteLock(0))
				try
				{
					_cacheLock.EnterWriteLock();
					using (Stream xmlDataFileInputStream = dataBlob.OpenRead())
						cachedDocument = new CachedXmlDocument(XDocument.Load(xmlDataFileInputStream), dataBlob.Properties.LastModified);
					if (_cachedDocuments.ContainsKey(xmlDocumentBlobName))
						_cachedDocuments[xmlDocumentBlobName] = cachedDocument;
					else
						_cachedDocuments.Add(xmlDocumentBlobName, cachedDocument);
				}
				finally
				{
					if (Interlocked.Decrement(ref _readersWaitingForUpdate) > 0)
					{
						_getDataLock.ExitWriteLock();
						_noGetDataWaits.WaitOne();
					}
					else
						_getDataLock.ExitWriteLock();
					_cacheLock.ExitWriteLock();
				}
			else
				try
				{
					_getDataLock.EnterReadLock();
					cachedDocument = _cachedDocuments[xmlDocumentBlobName];
				}
				finally
				{
					_getDataLock.ExitReadLock();
					if (Interlocked.Decrement(ref _readersWaitingForUpdate) == 0)
						_noGetDataWaits.Set();
				}

			return cachedDocument.XmlDocument;
		}
		[Obsolete]
		protected override void OnSaveXmlDocument(XDocument xmlDocument, string xmlDocumentBlobName)
		{
			CloudBlockBlob dataBlob = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(_connectionStringCloudSettingName))
														 .CreateCloudBlobClient()
														 .GetContainerReference("andrei15193")
														 .GetBlockBlobReference(xmlDocumentBlobName);

			try
			{
				_cacheLock.EnterWriteLock();
				dataBlob.UploadText(xmlDocument.ToString());
				if (_cachedDocuments.ContainsKey(xmlDocumentBlobName))
					_cachedDocuments[xmlDocumentBlobName] = new CachedXmlDocument(xmlDocument, dataBlob.Properties.LastModified);
				else
					_cachedDocuments.Add(xmlDocumentBlobName, new CachedXmlDocument(xmlDocument, dataBlob.Properties.LastModified));
			}
			finally
			{
				_cacheLock.ExitWriteLock();
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

		private struct CachedXmlDocument
		{
			public CachedXmlDocument(XDocument xmlDocument, DateTimeOffset? lastModifiedTime)
			{
				if (xmlDocument == null)
					throw new ArgumentNullException("xmlDocument");
				_xmlDocument = xmlDocument;
				_lastModifiedTime = lastModifiedTime;
			}

			public DateTimeOffset? LastModifiedTime
			{
				get
				{
					return _lastModifiedTime;
				}
			}
			public XDocument XmlDocument
			{
				get
				{
					return _xmlDocument;
				}
			}

			private readonly DateTimeOffset? _lastModifiedTime;
			private readonly XDocument _xmlDocument;
		}

		private readonly IDictionary<string, ReaderWriterLockSlim> _documentLocks = new SortedList<string, ReaderWriterLockSlim>();


		private int _readersWaitingForUpdate = 0;
		private readonly string _connectionStringCloudSettingName;
		private readonly AutoResetEvent _noGetDataWaits = new AutoResetEvent(false);
		private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
		private readonly ReaderWriterLockSlim _getDataLock = new ReaderWriterLockSlim();
		private readonly IDictionary<string, CachedXmlDocument> _cachedDocuments = new SortedDictionary<string, CachedXmlDocument>();
	}
}