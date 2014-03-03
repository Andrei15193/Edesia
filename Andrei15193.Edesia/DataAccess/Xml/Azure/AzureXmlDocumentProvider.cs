﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
namespace Andrei15193.Edesia.DataAccess.Xml.Azure
{
	public class AzureXmlDocumentProvider
		: XmlDocumentProvider
	{
		protected override XDocument OnLoadXmlDocument(string xmlDocumentBlobName)
		{
			CachedXmlDocument cachedDocument;
			CloudBlockBlob dataBlob = CloudStorageAccount.Parse(MvcApplication.EdesiaSettings.StorageSettings.StorageConnectionString)
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
		protected override void OnSaveXmlDocument(XDocument xmlDocument, string xmlDocumentBlobName)
		{
			CloudBlockBlob dataBlob = CloudStorageAccount.Parse(MvcApplication.EdesiaSettings.StorageSettings.StorageConnectionString)
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

		private static int _readersWaitingForUpdate = 0;
		private static readonly AutoResetEvent _noGetDataWaits = new AutoResetEvent(false);
		private static readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
		private static readonly ReaderWriterLockSlim _getDataLock = new ReaderWriterLockSlim();
		private static readonly IDictionary<string, CachedXmlDocument> _cachedDocuments = new SortedDictionary<string, CachedXmlDocument>();
	}
}