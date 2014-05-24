using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IDeliveryTaskRepository
	{
		IEnumerable<DeliveryTask> AddDeliveryTasks(IEnumerable<DeliveryTaskDetails> deliveryTasksDetails);
		IEnumerable<DeliveryTask> AddDeliveryTasks(params DeliveryTaskDetails[] deliveryTasksDetails);

		IEnumerable<DeliveryTask> GetUndergoingDeliveryTasks(IApplicationUserProvider applicationUserProvider, IDeliveryZoneProvider deliveryZoneProvider, IProductProvider productProvider, IOrderProvider orderProvider);
	}
}