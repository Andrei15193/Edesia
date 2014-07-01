using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IDeliveryRepository
	{
		Order Add(OrderDetails orderDatails);
		void Update(Order order);
		IEnumerable<Order> GetOrders(params OrderState[] orderStates);
		IEnumerable<Order> GetOrders(ApplicationUser recipient, params OrderState[] orderStates);

		void Add(string street);
		void Remove(string street);
		IEnumerable<string> GetStreets();
		IEnumerable<string> GetUnmappedStreets();

		void Add(DeliveryZone deliveryZone);
		void Update(DeliveryZone deliveryZone, string oldDeliveryZoneName);
		void Remove(DeliveryZone deliveryZone);
		DeliveryZone GetDeliveryZone(string name);
		IEnumerable<DeliveryZone> GetDeliveryZones();

		DeliveryTask Add(DeliveryTaskDetails deliveryTasksDetails);
		IEnumerable<DeliveryTask> Add(IEnumerable<DeliveryTaskDetails> deliveryTaskDetails);
		void Update(DeliveryTask deliveryTask);
		DeliveryTask GetDeliveryTask(int task);
		IEnumerable<DeliveryTask> GetDeliveryTasks(params TaskState[] taskStates);
		IEnumerable<DeliveryTask> GetDeliveryTasks(Employee employee, params TaskState[] taskStates);
	}
}