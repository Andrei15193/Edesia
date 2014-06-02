using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IOrderProvider
	{
		Order GetOrder(IApplicationUserProvider applicationUserProvider, IProductProvider productProvider, int orderNumber);
		Order GetOrder(IApplicationUserProvider applicationUserProvider, IProductProvider productProvider, int orderNumber, DateTime version);
		IEnumerable<string> GetUsedStreets();
		IEnumerable<Order> GetOrders(ApplicationUser applicationUser, IProductProvider _productProvider, params OrderState[] orderStates);
	}
}