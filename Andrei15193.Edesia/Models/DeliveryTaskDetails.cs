using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class DeliveryTaskDetails
	{
		public DeliveryTaskDetails(DateTime dateScheduled, DeliveryZone deliveryZone, IEnumerable<Order> ordersToDeliver)
		{
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			if (ordersToDeliver == null)
				throw new ArgumentNullException("ordersToDeliver");
			if (!ordersToDeliver.Any())
				throw new ArgumentException("Cannot be empty!", "ordersToDelvier");

			_dateScheduled = dateScheduled;
			_deliveryZone = deliveryZone;
			_ordersToDeliver = ordersToDeliver.Where(orderToDeliver => orderToDeliver != null).ToList();
		}
		public DeliveryTaskDetails(DateTime dateScheduled, DeliveryZone deliveryZone, params Order[] ordersToDeliver)
			: this(dateScheduled, deliveryZone, (IEnumerable<Order>)ordersToDeliver)
		{
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

		private readonly DateTime _dateScheduled;
		private readonly DeliveryZone _deliveryZone;
		private readonly IReadOnlyCollection<Order> _ordersToDeliver;
	}
}