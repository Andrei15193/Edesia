﻿using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class DeliveryTask
	{
		public DeliveryTask(int number, DateTime dateScheduled, DeliveryZone deliveryZone, bool isCancelled, IEnumerable<Order> ordersToDeliver)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			if (ordersToDeliver == null)
				throw new ArgumentNullException("ordersToDeliver");
			if (!ordersToDeliver.Any())
				throw new ArgumentException("Cannot be empty!", "ordersToDelvier");

			_number = number;
			_dateScheduled = dateScheduled;
			_deliveryZone = deliveryZone;
			_isCancelled = isCancelled;
			_ordersToDeliver = ordersToDeliver.Where(orderToDeliver => (orderToDeliver != null)).ToList();
		}
		public DeliveryTask(int number, DateTime dateScheduled, DeliveryZone deliveryZone, bool isCancelled, params Order[] ordersToDeliver)
			: this(number, dateScheduled, deliveryZone, isCancelled, (IEnumerable<Order>)ordersToDeliver)
		{
		}

		public int Number
		{
			get
			{
				return _number;
			}
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
		public void FinishTask()
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
		public static IEqualityComparer<DeliveryTask> IdentityComparer
		{
			get
			{
				return _identityComparer;
			}
		}

		private bool _isCancelled;
		private readonly int _number;
		private readonly DateTime _dateScheduled;
		private readonly DeliveryZone _deliveryZone;
		private readonly IReadOnlyCollection<Order> _ordersToDeliver;
		private static readonly IEqualityComparer<DeliveryTask> _identityComparer = new DeliveryTaskIdentityComparer();

		private sealed class DeliveryTaskIdentityComparer
			: IEqualityComparer<DeliveryTask>
		{
			#region IEqualityComparer<DeliveryTask> Members
			public bool Equals(DeliveryTask one, DeliveryTask another)
			{
				if (one == null)
					return (another == null);
				else
					return (another != null
							&& one._number == another._number);
			}
			public int GetHashCode(DeliveryTask value)
			{
				if (value == null)
					throw new ArgumentNullException("value");

				return value._number.GetHashCode();
			}
			#endregion
		}
	}
}