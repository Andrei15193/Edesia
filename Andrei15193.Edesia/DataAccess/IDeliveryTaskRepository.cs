using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	[Obsolete]
	public interface IDeliveryTaskRepository
	{
		IEnumerable<DeliveryTask> AddDeliveryTasks(IEnumerable<DeliveryTaskDetails> deliveryTasksDetails);
		IEnumerable<DeliveryTask> AddDeliveryTasks(params DeliveryTaskDetails[] deliveryTasksDetails);

		IEnumerable<DeliveryTask> GetDeliveryTasks(IApplicationUserProvider applicationUserProvider, IDeliveryZoneProvider deliveryZoneProvider, IProductProvider productProvider, IOrderProvider orderProvider, params TaskState[] taskStates);
		IEnumerable<DeliveryTask> GetDeliveryTasks(Employee employee, IApplicationUserProvider applicationUserProvider, IDeliveryZoneProvider deliveryZoneProvider, IProductProvider productProvider, IOrderProvider orderProvider, params TaskState[] taskStates);
		DeliveryTask GetDeliveryTask(int taskNumber, IApplicationUserProvider applicationUserProvider, IDeliveryZoneProvider deliveryZoneProvider, IProductProvider productProvider, IOrderProvider orderProvider);

		void CancelTask(int deliveryTaskNumber);

	}
}