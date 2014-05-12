using System;
using System.Xml.Linq;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public interface IXmlTransaction
		: IDisposable
	{
		XDocument XmlDocument
		{
			get;
		}
		void Commit();
	}
}