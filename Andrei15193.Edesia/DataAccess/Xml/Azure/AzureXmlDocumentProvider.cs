using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
namespace Andrei15193.Edesia.DataAccess.Xml.Azure
{
	public class AzureXmlDocumentProvider
		: IXmlDocumentProvider
	{
		#region IXmlDataProvider Members
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
				return _xmlDocumentSchemaSet;
			}
		}
		public XDocument LoadXmlDocument(string xmlDocumentFileName)
		{
			if (xmlDocumentFileName == null)
				throw new ArgumentNullException("xmlDocumentFileName");
			if (string.IsNullOrEmpty(xmlDocumentFileName) || string.IsNullOrWhiteSpace(xmlDocumentFileName))
				throw new ArgumentException("Filename cannot be empty or whitespace!", "xmlDocumentFileName");

			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(MvcApplication.EdesiaSettings.StorageSettings.StorageConnectionString);
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobClient.GetContainerReference("andrei15193");
			CloudBlockBlob dataBlob = container.GetBlockBlobReference(xmlDocumentFileName);
			CachedDocument cachedDocument;

			try
			{
				_cacheLock.EnterReadLock();

				if (_cachedDocuments.TryGetValue(xmlDocumentFileName, out cachedDocument)
					&& cachedDocument.LastModifiedTime == dataBlob.Properties.LastModified)
					return cachedDocument.Document;
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
						cachedDocument = new CachedDocument(XDocument.Load(xmlDataFileInputStream), dataBlob.Properties.LastModified);
					if (!ValidateSourceDocument)
						cachedDocument.Document.Validate(XmlDocumentSchemaSet, XmlDocumentError);

					_RaiseEvent(LoadingXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName));

					if (_cachedDocuments.ContainsKey(xmlDocumentFileName))
						_cachedDocuments[xmlDocumentFileName] = cachedDocument;
					else
						_cachedDocuments.Add(xmlDocumentFileName, cachedDocument);

					_RaiseEvent(LoadedXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName, cachedDocument.Document));
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
					_RaiseEvent(LoadingXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName));
					cachedDocument = _cachedDocuments[xmlDocumentFileName];
					_RaiseEvent(LoadedXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName, cachedDocument.Document));
				}
				finally
				{
					_getDataLock.ExitReadLock();
					if (Interlocked.Decrement(ref _readersWaitingForUpdate) == 0)
						_noGetDataWaits.Set();
				}

			return cachedDocument.Document;
		}
		public void SaveXmlDocument(XDocument xmlDocument, string xmlDocumentFileName)
		{
			if (xmlDocument == null)
				throw new ArgumentNullException("xmlDocument");
			if (xmlDocumentFileName == null)
				throw new ArgumentNullException("xmlDataFileName");
			if (string.IsNullOrEmpty(xmlDocumentFileName) || string.IsNullOrWhiteSpace(xmlDocumentFileName))
				throw new ArgumentException("Filename cannot be empty or whitespace!", "xmlDataFileName");

			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(MvcApplication.EdesiaSettings.StorageSettings.StorageConnectionString);
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobClient.GetContainerReference("andrei15193");
			CloudBlockBlob dataBlob = container.GetBlockBlobReference(xmlDocumentFileName);

			try
			{
				_cacheLock.EnterWriteLock();
				_RaiseEvent(SavingXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName, xmlDocument));
				xmlDocument.Validate(XmlDocumentSchemaSet, XmlDocumentError);

				dataBlob.UploadText(xmlDocument.ToString());
				if (_cachedDocuments.ContainsKey(xmlDocumentFileName))
					_cachedDocuments[xmlDocumentFileName] = new CachedDocument(xmlDocument, dataBlob.Properties.LastModified);
				else
					_cachedDocuments.Add(xmlDocumentFileName, new CachedDocument(xmlDocument, dataBlob.Properties.LastModified));
				_RaiseEvent(SavedXmlDocument, new XmlDocumentEventArgs(xmlDocumentFileName, xmlDocument));
			}
			finally
			{
				_cacheLock.ExitWriteLock();
			}
		}
		#endregion

		private struct CachedDocument
		{
			public CachedDocument(XDocument document, DateTimeOffset? lastModifiedTime)
			{
				if (document == null)
					throw new ArgumentNullException("document");
				_document = document;
				_lastModifiedTime = lastModifiedTime;
			}

			public DateTimeOffset? LastModifiedTime
			{
				get
				{
					return _lastModifiedTime;
				}
			}
			public XDocument Document
			{
				get
				{
					return _document;
				}
			}

			private readonly DateTimeOffset? _lastModifiedTime;
			private readonly XDocument _document;
		}
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
		private int _readersWaitingForUpdate = 0;
		private readonly AutoResetEvent _noGetDataWaits = new AutoResetEvent(false);
		private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
		private readonly ReaderWriterLockSlim _getDataLock = new ReaderWriterLockSlim();
		private readonly IDictionary<string, CachedDocument> _cachedDocuments = new SortedDictionary<string, CachedDocument>();
		private readonly XmlSchemaSet _xmlDocumentSchemaSet = new XmlSchemaSet();
	}
}