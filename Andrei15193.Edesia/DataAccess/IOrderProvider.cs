using System;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IOrderProvider
	{
		Order GetOrder(IApplicationUserProvider applicationUserProvider, IProductProvider productProvider, int orderNumber, DateTime version);
	}
}