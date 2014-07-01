using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.ConstraintSatisfaction;
using Andrei15193.ConstraintSatisfaction.Tuples;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Collections;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Models.Collections;
using Andrei15193.Edesia.ViewModels.DeliveryTask;
namespace Andrei15193.Edesia.Controllers
{
	public class DeliveryTaskController
		: ApplicationController
	{
		[ChildActionOnly]
		public ActionResult Default()
		{
			return View(_deliveryRepository.GetDeliveryTasks(TaskState.InProgress, TaskState.Scheduled));
		}

		[HttpGet, Authorize, Role(typeof(Administrator))]
		public ActionResult Schedule()
		{
			IDictionary<DeliveryZone, IEnumerable<Order>> ordersByDeliveryZone = new Dictionary<DeliveryZone, IEnumerable<Order>>();
			IDictionary<int, Order> remainingOrders = new SortedList<int, Order>();
			foreach (Order order in _deliveryRepository.GetOrders(OrderState.Pending))
				remainingOrders.Add(order.Number, order);

			foreach (DeliveryZone deliveryZone in _deliveryRepository.GetDeliveryZones().Where(deliveryZone => deliveryZone.Assignee != null))
			{
				ICollection<Order> orders = new LinkedList<Order>(remainingOrders.Values.Where(order => deliveryZone.Streets.Contains(order.DeliveryAddress.Street) && order.TotalCapacity <= deliveryZone.Assignee.TransportCapacity));
				if (orders.Count > 0)
				{
					ordersByDeliveryZone.Add(deliveryZone, orders);
					foreach (Order order in orders)
						remainingOrders.Remove(order.Number);
				}
			}

			IEnumerable<DeliveryTaskDetails> deliveryTasksDetails = ordersByDeliveryZone.AsParallel()
																						.SelectMany(_GetDeliveryTasks)
																						.ToList();
			foreach (DeliveryTaskDetails deliveryTaskDetails in deliveryTasksDetails)
				foreach (Order orderToDeliver in deliveryTaskDetails.OrdersToDeliver)
					orderToDeliver.State = OrderState.Scheduled;
			_deliveryRepository.Add(deliveryTasksDetails);

			return RedirectToAction("Default", "Delivery");
		}

		[HttpGet, Authorize, Role(typeof(Administrator))]
		public ActionResult Cancel(int task, string returnUrl)
		{
			DeliveryTask deliveryTask = _deliveryRepository.GetDeliveryTask(task);

			deliveryTask.CancelTask();
			_deliveryRepository.Update(deliveryTask);

			if (Url.IsLocalUrl(returnUrl))
				return Redirect(returnUrl);
			else
				return RedirectToAction("Default", "Delivery");
		}

		[HttpGet, Authorize, Role(typeof(Employee))]
		public ActionResult Start(int task)
		{
			DeliveryTask deliveryTask = _deliveryRepository.GetDeliveryTask(task);

			if (deliveryTask == null || !ApplicationUser.IdentityComparer.Equals(deliveryTask.DeliveryZone.Assignee, User))
				return RedirectToAction("Forbidden", "Error");

			deliveryTask.StartTask();
			_deliveryRepository.Update(deliveryTask);

			return RedirectToAction("Dashboard", "DeliveryTask");
		}

		[HttpGet, Authorize, Role(typeof(Employee))]
		public ActionResult Finish(int task)
		{
			DeliveryTask deliveryTask = _deliveryRepository.GetDeliveryTask(task);

			if (deliveryTask == null || !ApplicationUser.IdentityComparer.Equals(deliveryTask.DeliveryZone.Assignee, User))
				return RedirectToAction("Forbidden", "Error");

			deliveryTask.FinishTask();
			_deliveryRepository.Update(deliveryTask);

			return RedirectToAction("Dashboard", "DeliveryTask");
		}

		[HttpGet, Authorize, Role(typeof(Employee))]
		public ActionResult Dashboard()
		{
			Employee employee = User.TryGetRole<Employee>();
			return View(new DashboardViewModel(_deliveryRepository.GetDeliveryTasks(employee, TaskState.InProgress, TaskState.Scheduled, TaskState.Completed)));
		}

		[HttpGet, Authorize, Role(typeof(Employee))]
		public JsonResult PendingCountJson()
		{
			return Json(new
				{
					Count = _deliveryRepository.GetDeliveryTasks(User.TryGetRole<Employee>(), TaskState.Scheduled, TaskState.InProgress).Count()
				},
				JsonRequestBehavior.AllowGet);
		}

		private IEnumerable<DeliveryTaskDetails> _GetDeliveryTasks(KeyValuePair<DeliveryZone, IEnumerable<Order>> ordersByDeliveryZone)
		{
			DateTime now = DateTime.Now;

			double sum = 0;
			int maximumNumberOfOrdersInpartition = ordersByDeliveryZone.Value
																	   .OrderBy(order => order.TotalCapacity)
																	   .TakeWhile(order => (sum += order.TotalCapacity) <= ordersByDeliveryZone.Key.Assignee.TransportCapacity)
																	   .Count();
			if (ordersByDeliveryZone.Value.Count() == maximumNumberOfOrdersInpartition)
				return new DeliveryTaskDetails[] { new DeliveryTaskDetails(now, ordersByDeliveryZone.Key, ordersByDeliveryZone.Value) };

			int numberOfPartitions;
			double tot = (ordersByDeliveryZone.Value.Sum(order => order.TotalCapacity) / ordersByDeliveryZone.Key.Assignee.TransportCapacity);

			if (Math.Floor(tot) == tot)
				numberOfPartitions = (int)tot + 1;
			else
				numberOfPartitions = (int)Math.Ceiling(tot);

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

				return solutionEnumerator.Current
										 .Values
										 .Select((partition) => new DeliveryTaskDetails(now, ordersByDeliveryZone.Key, partition));
			}
			finally
			{
				if (solutionEnumerator != null)
					solutionEnumerator.Dispose();
			}
		}

		private bool _OrdersPartitionConstraint(IPair<Order, Order> orders)
		{
			return (orders.First.Number < orders.Second.Number);
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
				yield return BinaryConstraint.Create<int, Order>(order.Value.Number, order.Next.Value.Number, (orderSequence => orderSequence.First != null));

				do
				{
					yield return BinaryConstraint.Create<int, Order>(order.Value.Number, order.Next.Value.Number, _OrderSequenceConstraint);
					order = order.Next;
				} while (order.Next != null);
			}
		}
		private bool _OrderSequenceConstraint(IPair<Order, Order> orderSequence)
		{
			if (orderSequence.First == null)
				return (orderSequence.Second == null);
			else
				return (orderSequence.Second == null || orderSequence.First.Number < orderSequence.Second.Number);
		}

		private IEnumerable<int> _Range(int from = 0, int to = 0)
		{
			for (int value = from; value < to; value++)
				yield return value;
		}

		private readonly IConstraintSatisfactionSearch<int, Order> _ordersPartitionSearch = new ForwardCheckingSearch<int, Order>();
		private readonly IConstraintSatisfactionSearch<int, OrdersPartition> _disjointOrdersPartitionSearch = new ForwardCheckingSearch<int, OrdersPartition>();
		private readonly IDeliveryRepository _deliveryRepository = (IDeliveryRepository)MvcApplication.DependencyContainer["deliveryRepo"];
		//private readonly IOrderRepository _orderRepository = (IOrderRepository)MvcApplication.DependencyContainer["orderRepository"];
		//private readonly IProductProvider _productProvider = (IProductProvider)MvcApplication.DependencyContainer["productRepository"];
		//private readonly IDeliveryZoneProvider _deliveryZoneRepository = (IDeliveryZoneProvider)MvcApplication.DependencyContainer["deliveryRepository"];
		//private readonly IApplicationUserProvider _applicationUserProvider = (IApplicationUserProvider)MvcApplication.DependencyContainer["applicationUserRepository"];
		//private readonly IDeliveryTaskRepository _deliveryTaskRepository = (IDeliveryTaskRepository)MvcApplication.DependencyContainer["deliveryTaskRepository"];
	}
}