using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models.Collections
{
	public class OrdersPartition
		: IEnumerable<Order>
	{
		public OrdersPartition(int partitionNumber, IEnumerable<Order> orders)
		{
			if (orders == null)
				throw new ArgumentNullException("orders");

			_partitionNumber = partitionNumber;

			_orders = new SortedList<int, Order>(orders.Count());
			foreach (Order order in orders)
				_orders.Add(order.OrderNumber, order);
		}

		#region IEnumerable<Order> Members
		public IEnumerator<Order> GetEnumerator()
		{
			return _orders.Values.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		public int PartitionNumber
		{
			get
			{
				return _partitionNumber;
			}
		}
		public double TotalCapacity
		{
			get
			{
				return _orders.Values.Sum(order => order.TotalCapacity);
			}
		}
		public bool AreDisjoint(OrdersPartition other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			return _orders.Keys.All(orderNumber => !other._orders.ContainsKey(orderNumber));
		}

		private readonly int _partitionNumber;
		private readonly IDictionary<int, Order> _orders;
	}
}