using System;
using System.Collections.Generic;
using System.Linq;
using Andrei15193.Edesia.Collections;
namespace Andrei15193.Edesia.Models
{
	public class DeliveryTask
	{
		public DeliveryTask(int deliveryTaskNumber, DateTime dateScheduled, DeliveryZone deliveryZone, bool isCancelled, IEnumerable<Order> ordersToDeliver)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			if (ordersToDeliver == null)
				throw new ArgumentNullException("ordersToDeliver");
			if (!ordersToDeliver.Any())
				throw new ArgumentException("Cannot be empty!", "ordersToDelvier");

			_deliveryTaskNumber = deliveryTaskNumber;
			_dateScheduled = dateScheduled;
			_deliveryZone = deliveryZone;
			_isCancelled = isCancelled;
			_ordersToDeliver = new ReadOnlyCollection<Order>(ordersToDeliver.Where(orderToDeliver => orderToDeliver != null));
		}
		public DeliveryTask(int deliveryTaskNumber, DateTime dateScheduled, DeliveryZone deliveryZone, bool isCancelled, params Order[] ordersToDeliver)
			: this(deliveryTaskNumber, dateScheduled, deliveryZone, isCancelled, (IEnumerable<Order>)ordersToDeliver)
		{
		}

		public bool IsCancelled
		{
			get
			{
				return _isCancelled;
			}
		}
		public TaskState State
		{
			get
			{
				if (_isCancelled)
					return TaskState.Cancelled;

				ISet<OrderState> orderStates = new SortedSet<OrderState>(_ordersToDeliver.Select(orderToDelvier => orderToDelvier.State));

				if (orderStates.Count == 1)
				{
					if (orderStates.Contains(OrderState.Scheduled))
						return TaskState.Scheduled;
					if (orderStates.Contains(OrderState.EnRoute))
						return TaskState.InProgress;
					if (orderStates.Contains(OrderState.Delivered))
						return TaskState.Completed;
				}

				return TaskState.Unknown;
			}
		}
		public int DeliveryTaskNumber
		{
			get
			{
				return _deliveryTaskNumber;
			}
		}
		public DateTime DateScheduled
		{
			get
			{
				return _dateScheduled;
			}
		}
		public DeliveryZone DeliveryZone
		{
			get
			{
				return _deliveryZone;
			}
		}
		public IReadOnlyCollection<Order> OrdersToDeliver
		{
			get
			{
				return _ordersToDeliver;
			}
		}
		public void StartTask()
		{
			if (_isCancelled)
				throw new InvalidOperationException("The current delivery task is cancelled!");

			foreach (Order orderToDeliver in _ordersToDeliver)
				orderToDeliver.State = OrderState.EnRoute;
		}
		public void CompleteTask()
		{
			if (_isCancelled)
				throw new InvalidOperationException("The current delivery task is cancelled!");

			foreach (Order orderToDeliver in _ordersToDeliver)
				orderToDeliver.State = OrderState.Delivered;
		}
		public void CancelTask()
		{
			_isCancelled = true;
			foreach (Order orderToDeliver in _ordersToDeliver)
				orderToDeliver.State = OrderState.Pending;
		}

		private bool _isCancelled;
		private readonly int _deliveryTaskNumber;
		private readonly DateTime _dateScheduled;
		private readonly DeliveryZone _deliveryZone;
		private readonly IReadOnlyCollection<Order> _ordersToDeliver;
	}
}