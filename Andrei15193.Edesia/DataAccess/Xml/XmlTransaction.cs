using System;
using System.Xml.Linq;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlTransaction
		: IXmlTransaction
	{
		public XmlTransaction(XDocument xmlDocument, Action saveAction, Action releaseLockAction)
		{
			if (xmlDocument == null)
				throw new ArgumentNullException("xmlDocument");
			if (saveAction == null)
				throw new ArgumentNullException("saveAction");
			if (releaseLockAction == null)
				throw new ArgumentNullException("releaseLockAction");

			_xmlDocument = xmlDocument;
			_saveAction = saveAction;
			_releaseLockAction = releaseLockAction;
		}

		#region IXmlTransaction Members
		public XDocument XmlDocument
		{
			get
			{
				if (_isDisposed)
					throw new ObjectDisposedException("Instance has been dispozed!");

				return _xmlDocument;
			}
		}
		public void Commit()
		{
			try
			{
				_saveAction();
			}
			finally
			{
				Dispose();
			}
		}
		#endregion
		#region IDisposable Members
		public void Dispose()
		{
			if (!_isDisposed)
			{
				_releaseLockAction();
				_isDisposed = true;
			}
		}
		#endregion

		private bool _isDisposed = false;
		private readonly XDocument _xmlDocument;
		private readonly Action _saveAction;
		private readonly Action _releaseLockAction;
	}
}