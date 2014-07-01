using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	[Obsolete]
	public interface IOrderRepository
		: IOrderProvider
	{
		Order PlaceOrder(OrderDetails orderDateils);
		IEnumerable<Order> GetOrders(IApplicationUserProvider applicationUserProvider, IProductProvider productProvider, params OrderState[] orderStates);
		void UpdateOrders(IEnumerable<Order> orders);
	}
}