using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IOrderRepository
		: IOrderProvider
	{
		Order PlaceOrder(OrderDetails orderDateils);
		IEnumerable<Order> GetOrders(OrderState orderState, IApplicationUserProvider applicationUserProvider, IProductProvider productProvider);
		void UpdateOrders(IEnumerable<Order> orders, OrderState orderState);

		IEnumerable<string> GetUsedAddresses();
	}
}