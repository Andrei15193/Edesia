using System;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public interface IExclusiveXmlTransaction
		: ISharedXmlTransaction
	{
		void Commit();
	}
}