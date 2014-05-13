using System;
using System.Collections.Generic;
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

		public override IXmlTransaction BeginXmlTransaction(string xmlDocumentName, XmlSchemaSet xmlSchemaSet = null)
		{
			SpinLock xmlDocumentSpinLock;
			CloudBlockBlob dataBlob = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(_connectionStringCloudSettingName))
														 .CreateCloudBlobClient()
														 .GetContainerReference("andrei15193")
														 .GetBlockBlobReference(xmlDocumentName);

			lock (_documentSpinLocksLock)
				using (Stream stream = dataBlob.OpenRead())
				{
					XDocument xmlDocument = XDocument.Load(stream);

					if (xmlSchemaSet != null)
						Validate(xmlDocument, xmlSchemaSet);

					if (!_documentSpinLocks.TryGetValue(xmlDocumentName, out xmlDocumentSpinLock))
					{
						xmlDocumentSpinLock = new SpinLock(enableThreadOwnerTracking: false);
						_documentSpinLocks.Add(xmlDocumentName, xmlDocumentSpinLock);
					}

					bool lockTaken = false;
					xmlDocumentSpinLock.Enter(ref lockTaken);

					return new XmlTransaction(xmlDocument, _SaveXmlDocument(dataBlob, xmlDocument, xmlSchemaSet), _ReleaseSpinLock(xmlDocumentSpinLock));
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

		private int _readersWaitingForUpdate = 0;
		private readonly string _connectionStringCloudSettingName;
		private readonly AutoResetEvent _noGetDataWaits = new AutoResetEvent(false);
		private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
		private readonly ReaderWriterLockSlim _getDataLock = new ReaderWriterLockSlim();
		private readonly IDictionary<string, CachedXmlDocument> _cachedDocuments = new SortedDictionary<string, CachedXmlDocument>();
	}
}