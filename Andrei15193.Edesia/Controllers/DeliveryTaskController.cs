using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.ConstraintSatisfaction;
using Andrei15193.ConstraintSatisfaction.Tuples;
using Andrei15193.Edesia.Collections;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Models.Collections;
namespace Andrei15193.Edesia.Controllers
{
	public class DeliveryTaskController
		: ApplicationController
	{
		[ChildActionOnly]
		public ActionResult Default()
		{
			return View(_deliveryTaskRepository.GetDeliveryTasks(_applicationUserProvider, _deliveryRepository, _productProvider, _orderRepository, TaskState.InProgress, TaskState.Scheduled));
		}

		[HttpGet]
		public ActionResult Schedule()
		{
			IDictionary<DeliveryZone, IEnumerable<Order>> ordersByDeliveryZone = new Dictionary<DeliveryZone, IEnumerable<Order>>();
			IDictionary<int, Order> remainingOrders = new SortedList<int, Order>();
			foreach (Order order in _orderRepository.GetOrders(_applicationUserProvider, _productProvider, OrderState.Pending))
				remainingOrders.Add(order.OrderNumber, order);

			foreach (DeliveryZone deliveryZone in _deliveryRepository.GetDeliveryZones(_applicationUserProvider).Where(deliveryZone => deliveryZone.Assignee != null))
			{
				ICollection<Order> orders = new LinkedList<Order>(remainingOrders.Values.Where(order => deliveryZone.Addresses.Contains(order.DeliveryAddress) && order.TotalCapacity <= deliveryZone.Assignee.TransportCapacity));
				if (orders.Count > 0)
				{
					ordersByDeliveryZone.Add(deliveryZone, orders);
					foreach (Order order in orders)
						remainingOrders.Remove(order.OrderNumber);
				}
			}

			IEnumerable<DeliveryTask> deliveryTasks =
				_deliveryTaskRepository.AddDeliveryTasks(ordersByDeliveryZone.AsParallel()
																			 .SelectMany(_GetDeliveryTasks)
																			 .ToList());

			_orderRepository.UpdateOrders(ordersByDeliveryZone.Values.SelectMany(orders => orders), OrderState.Scheduled);

			return RedirectToAction("Default", "Delivery");
		}

		private IEnumerable<DeliveryTaskDetails> _GetDeliveryTasks(KeyValuePair<DeliveryZone, IEnumerable<Order>> ordersByDeliveryZone)
		{
			DateTime now = DateTime.Now;

			int sum = 0;
			int maximumNumberOfOrdersInpartition = ordersByDeliveryZone.Value
																	   .OrderBy(order => order.TotalCapacity)
																	   .TakeWhile(order => (sum += order.TotalCapacity) <= ordersByDeliveryZone.Key.Assignee.TransportCapacity)
																	   .Count();
			if (ordersByDeliveryZone.Value.Count() == maximumNumberOfOrdersInpartition)
				return new DeliveryTaskDetails[] { new DeliveryTaskDetails(now, "Deliver", "Deliver!", ordersByDeliveryZone.Key, ordersByDeliveryZone.Value) };

			int numberOfPartitions;
			decimal tot = (ordersByDeliveryZone.Value.Sum(order => order.TotalCapacity) / (decimal)ordersByDeliveryZone.Key.Assignee.TransportCapacity);

			if (decimal.Floor(tot) == tot)
				numberOfPartitions = (int)tot + 1;
			else
				numberOfPartitions = (int)decimal.Ceiling(tot);

			ICollection<OrdersPartition> partitions = new LinkedList<OrdersPartition>();
			BinaryConstraints<int, Order> ordersPartitionBinaryConstraints = BinaryConstraints.Create(_Range(from: 0, to: maximumNumberOfOrdersInpartition - 1).Select(orderIndex => BinaryConstraint.Create<int, Order>(orderIndex, orderIndex + 1, _OrdersPartitionConstraint)));
			Constraint<IReadOnlyDictionary<int, Order>> solutionConstraint = Constraint.Create<IReadOnlyDictionary<int, Order>>(solution => (solution.Values.Sum(order => order.TotalCapacity) < ordersByDeliveryZone.Key.Assignee.TransportCapacity));

			for (int ordersInPartition = maximumNumberOfOrdersInpartition; ordersInPartition >= 1; ordersInPartition--)
				foreach (IEnumerable<Order> partition in _ordersPartitionSearch.Find(_Range(from: 0, to: ordersInPartition).Select(orderIndex => KeyValuePair.Create(orderIndex, ordersByDeliveryZone.Value)),
																					 ordersPartitionBinaryConstraints,
																					 solutionConstraint).Select(solution => solution.Values))
					partitions.Add(new OrdersPartition(partitions.Count, partition));

			BinaryConstraints<int, OrdersPartition> partitionsBinaryConstraints = BinaryConstraints.Create(_Range(from: 0, to: ordersByDeliveryZone.Value.Count() - 1).Select(partitionIndex => BinaryConstraint.Create<int, OrdersPartition>(partitionIndex, partitionIndex + 1, _PartitionsConstraints)));

			IEnumerator<IReadOnlyDictionary<int, OrdersPartition>> solutionEnumerator = null;
			try
			{
				do
				{
					if (solutionEnumerator != null)
						solutionEnumerator.Dispose();
					solutionEnumerator = _disjointOrdersPartitionSearch.Find(_Range(from: 0, to: numberOfPartitions).Select(partitionIndex => KeyValuePair.Create<int, IEnumerable<OrdersPartition>>(partitionIndex, partitions)),
																			 partitionsBinaryConstraints)
																	   .Where(solution => solution.Values.Sum(partition => partition.Count()) == ordersByDeliveryZone.Value.Count())
																	   .GetEnumerator();
					numberOfPartitions++;
				} while (!solutionEnumerator.MoveNext());

				return solutionEnumerator.Current.Values.Select(partition => new DeliveryTaskDetails(now, "Deliver!", "Deliver I say!", ordersByDeliveryZone.Key, partition));
			}
			finally
			{
				if (solutionEnumerator != null)
					solutionEnumerator.Dispose();
			}
		}

		private bool _OrdersPartitionConstraint(IPair<Order, Order> orders)
		{
			return (orders.First.OrderNumber < orders.Second.OrderNumber);
		}
		private bool _PartitionsConstraints(IPair<OrdersPartition, OrdersPartition> partitions)
		{
			return (partitions.First.PartitionNumber < partitions.Second.PartitionNumber && partitions.First.AreDisjoint(partitions.Second));
		}

		private IEnumerable<BinaryConstraint<int, OrdersPartition>> _GetBinaryConstraints(IEnumerable<int> ordersPartitionIndexes)
		{
			if (ordersPartitionIndexes.Skip(1).Any())
				yield return BinaryConstraint.Create<int, OrdersPartition>(1, 2, partitions => partitions.First != null);

			for (int firstPartitionIndex = 0, ordersPartitionsCount = ordersPartitionIndexes.Count() - 1; firstPartitionIndex < ordersPartitionsCount; firstPartitionIndex++)
				for (int secondPartitionIndex = firstPartitionIndex + 1; secondPartitionIndex < ordersPartitionsCount; secondPartitionIndex++)
					yield return BinaryConstraint.Create<int, OrdersPartition>(firstPartitionIndex + 1,
																			   secondPartitionIndex + 1,
																			   _OrdersPartitionsConstraint);
		}
		private bool _OrdersPartitionsConstraint(IPair<OrdersPartition, OrdersPartition> partitions)
		{
			if (partitions.First == null)
				return (partitions.Second == null);
			else
				return (partitions.Second == null || (partitions.First.PartitionNumber < partitions.Second.PartitionNumber && partitions.First.AreDisjoint(partitions.Second)));
		}

		private IEnumerable<BinaryConstraint<int, Order>> _GetBinaryConstraints(LinkedListNode<Order> order)
		{
			if (order != null && order.Next != null)
			{
				yield return BinaryConstraint.Create<int, Order>(order.Value.OrderNumber, order.Next.Value.OrderNumber, (orderSequence => orderSequence.First != null));

				do
				{
					yield return BinaryConstraint.Create<int, Order>(order.Value.OrderNumber, order.Next.Value.OrderNumber, _OrderSequenceConstraint);
					order = order.Next;
				} while (order.Next != null);
			}
		}
		private bool _OrderSequenceConstraint(IPair<Order, Order> orderSequence)
		{
			if (orderSequence.First == null)
				return (orderSequence.Second == null);
			else
				return (orderSequence.Second == null || orderSequence.First.OrderNumber < orderSequence.Second.OrderNumber);
		}
		
		private IEnumerable<int> _Range(int from = 0, int to = 0)
		{
			for (int value = from; value < to; value++)
				yield return value;
		}

		private readonly IConstraintSatisfactionSearch<int, Order> _ordersPartitionSearch = new ForwardCheckingSearch<int, Order>();
		private readonly IConstraintSatisfactionSearch<int, OrdersPartition> _disjointOrdersPartitionSearch = new ForwardCheckingSearch<int, OrdersPartition>();
		private readonly IOrderRepository _orderRepository = (IOrderRepository)MvcApplication.DependencyContainer["orderRepository"];
		private readonly IProductProvider _productProvider = (IProductProvider)MvcApplication.DependencyContainer["productRepository"];
		private readonly IDeliveryZoneProvider _deliveryRepository = (IDeliveryZoneProvider)MvcApplication.DependencyContainer["deliveryRepository"];
		private readonly IApplicationUserProvider _applicationUserProvider = (IApplicationUserProvider)MvcApplication.DependencyContainer["applicationUserRepository"];
		private readonly IDeliveryTaskRepository _deliveryTaskRepository = (IDeliveryTaskRepository)MvcApplication.DependencyContainer["deliveryTaskRepository"];
	}
}