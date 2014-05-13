using System;
using System.Xml.Linq;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlTransaction
		: IExclusiveXmlTransaction
	{
		public XmlTransaction(XDocument xmlDocument, Action commitAction = null, Action disposeAction = null)
		{
			if (xmlDocument == null)
				throw new ArgumentNullException("xmlDocument");

			_xmlDocument = xmlDocument;
			_commit = commitAction;
			_disposeAction = disposeAction;
		}

		#region IExclusiveXmlTransaction Members
		public void Commit()
		{
			try
			{
				if (_commit != null)
					_commit();
			}
			finally
			{
				Dispose();
			}
		}
		#endregion
		#region ISharedXmlTransaction Members
		public XDocument XmlDocument
		{
			get
			{
				if (_isDisposed)
					throw new ObjectDisposedException("Instance has been dispozed!");

				return _xmlDocument;
			}
		}
		#endregion
		#region IDisposable Members
		public void Dispose()
		{
			if (!_isDisposed)
			{
				if (_disposeAction != null)
					_disposeAction();
				_isDisposed = true;
			}
		}
		#endregion

		private bool _isDisposed = false;
		private readonly XDocument _xmlDocument;
		private readonly Action _commit;
		private readonly Action _disposeAction;
	}
}