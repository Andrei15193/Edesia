using System;
using System.Xml.Linq;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public interface ISharedXmlTransaction
		: IDisposable
	{
		XDocument XmlDocument
		{
			get;
		}
	}
}