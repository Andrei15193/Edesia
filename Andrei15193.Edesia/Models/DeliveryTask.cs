using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class DeliveryTask
	{
		public DeliveryTask(int deliveryTaskNumber, DateTime dateScheduled, string title, string description, DeliveryZone deliveryZone, IEnumerable<Order> ordersToDeliver)
		{
			if (title == null)
				throw new ArgumentNullException("title");
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Cannot be empty or whitespace!", "title");

			if (description == null)
				throw new ArgumentNullException("description");
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Cannot be empty or whitespace!", "description");

			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			if (ordersToDeliver == null)
				throw new ArgumentNullException("ordersToDeliver");
			if (!ordersToDeliver.Any())
				throw new ArgumentException("Cannot be empty!", "ordersToDelvier");

			_deliveryTaskNumber = deliveryTaskNumber;
			_dateScheduled = dateScheduled;
			_title = title;
			_description = description;
			_deliveryZone = deliveryZone;
			_ordersToDeliver = ordersToDeliver.Where(orderToDeliver => ordersToDeliver != null).ToList();
		}
		public DeliveryTask(int deliveryTaskNumber, DateTime dateScheduled, string title, string description, DeliveryZone deliveryZone, params Order[] ordersToDeliver)
			: this(deliveryTaskNumber, dateScheduled, title, description, deliveryZone, (IEnumerable<Order>)ordersToDeliver)
		{
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
		public string Title
		{
			get
			{
				return _title;
			}
		}
		public string Description
		{
			get
			{
				return _description;
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

		private readonly int _deliveryTaskNumber;
		private readonly string _title;
		private readonly string _description;
		private readonly DateTime _dateScheduled;
		private readonly DeliveryZone _deliveryZone;
		private readonly IReadOnlyCollection<Order> _ordersToDeliver;
	}
}