using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Andrei15193.ConstraintSatisfaction;
using Andrei15193.ConstraintSatisfaction.Tuples;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Collections;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.Delivery;
namespace Andrei15193.Edesia.Controllers
{
	[ConfirmAccess(typeof(Administrator))]
	public class DeliveryController
		: ApplicationController
	{
		[HttpGet]
		public ActionResult Planning()
		{
			return View(new DeliveryPlanningViewModel(new DeliveryZonesViewModel(_deliveryRepository.GetUnmappedAddresses(), _deliveryRepository.GetDeliveryZones(_applicationUserProvider), _GetUnuesdAddresses())));
		}
		[HttpGet]
		public ActionResult ManageDeliveryZones()
		{
			return View(new DeliveryZonesViewModel(_deliveryRepository.GetUnmappedAddresses(), _deliveryRepository.GetDeliveryZones(_applicationUserProvider), _GetUnuesdAddresses()));
		}
		[ChildActionOnly]
		public ActionResult DeliveryZones()
		{
			return View(new DeliveryZonesViewModel(_deliveryRepository.GetUnmappedAddresses(), _deliveryRepository.GetDeliveryZones(_applicationUserProvider), _GetUnuesdAddresses()));
		}

		[HttpGet]
		public ActionResult AddAddress()
		{
			return View();
		}
		[HttpPost]
		public ActionResult AddAddress(AddAddressViewModel addAddressViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.AddAddress(addAddressViewModel.Address);
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueAddressException uniqueAddressException = aggregatedException as UniqueAddressException;

						if (uniqueAddressException != null)
							ModelState.AddModelError("Address", string.Format(ErrorStrings.AddressTextBox_InvalidDuplicateValue_Format, uniqueAddressException.ConflictingValue));
					}

					return View(addAddressViewModel);
				}
			}
			else
				return View(addAddressViewModel);
		}

		[HttpGet]
		public ActionResult RemoveAddress()
		{
			RemoveAddressViewModel removeAddressesViewModel = new RemoveAddressViewModel();

			foreach (string unusedAddress in _GetUnuesdAddresses())
				removeAddressesViewModel.UnusedAddresses.Add(unusedAddress);

			return View(removeAddressesViewModel);
		}
		[HttpPost]
		public ActionResult RemoveAddress(RemoveAddressViewModel removeAddressViewModel)
		{
			if (ModelState.IsValid)
			{
				_deliveryRepository.RemoveAddress(removeAddressViewModel.AddressToRemove);
				return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
			}
			else
				return View(removeAddressViewModel);
		}

		[HttpGet]
		public ActionResult AddDeliveryZone()
		{
			return View(new DeliveryZoneViewModel(_deliveryRepository.GetUnmappedAddresses().Select(unmappedAddress => new KeyValuePair<string, bool>(unmappedAddress, false)),
												  _applicationUserProvider.GetEmployees())
				{
					SubmitButtonText = AddDeliveryZoneViewStrings.SubmitButton_DisplayName
				});
		}
		[HttpPost]
		public ActionResult AddDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.AddDeliveryZone(_GetDeliveryZone(deliveryZoneViewModel));
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueDeliveryZoneNameException uniqueDeliveryZoneNameException = aggregatedException as UniqueDeliveryZoneNameException;

						if (uniqueDeliveryZoneNameException != null)
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueDeliveryZoneNameException.ConflictingValue));
					}
				}
			}

			deliveryZoneViewModel.AvailableAddresses.Clear();
			foreach (string unmappedAddress in _deliveryRepository.GetUnmappedAddresses())
				deliveryZoneViewModel.AvailableAddresses.Add(new KeyValuePair<string, bool>(unmappedAddress, Request.Form.AllKeys.Contains("checkbox " + unmappedAddress)));

			return View(deliveryZoneViewModel);
		}

		[HttpGet]
		public ActionResult GetDeliveryZoneName()
		{
			return View(_deliveryRepository.GetDeliveryZones(_applicationUserProvider));
		}
		[HttpGet]
		public ActionResult EditDeliveryZone(string deliveryZoneName)
		{
			if (deliveryZoneName != null)
			{
				DeliveryZone deliveryZoneFound = _deliveryRepository.GetDeliveryZones(_applicationUserProvider).FirstOrDefault(deliveryZone => string.Equals(deliveryZoneName, deliveryZone.Name, StringComparison.OrdinalIgnoreCase));
				if (deliveryZoneFound != null)
				{
					DeliveryZoneViewModel deliveryZoneViewModel = new DeliveryZoneViewModel(deliveryZoneFound.Addresses.Select(address => new KeyValuePair<string, bool>(address, true))
																											 .Concat(_deliveryRepository.GetUnmappedAddresses().Select(address => new KeyValuePair<string, bool>(address, false))),
																							_applicationUserProvider.GetEmployees())
					{
						DeliveryZoneName = deliveryZoneFound.Name,
						DeliveryZoneColour = deliveryZoneFound.Colour.ToString(),
						DeliveryZoneOldName = deliveryZoneName,
						SelectedEmployeeEMailAddress = (deliveryZoneFound.Assignee == null ? null : deliveryZoneFound.Assignee.EMailAddress),
						SubmitButtonText = EditDeliveryZoneViewStrings.SubmitButton_DisplayName
					};

					return View(deliveryZoneViewModel);
				}
				else
					ModelState.AddModelError("deliveryZoneName", ErrorStrings.DeliveryZoneNameComboBox_InvalidValue);
			}

			return View("GetDeliveryZoneName", _deliveryRepository.GetDeliveryZones(_applicationUserProvider));
		}
		[HttpPost]
		public ActionResult EditDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.UpdateDeliveryZone(_GetDeliveryZone(deliveryZoneViewModel), deliveryZoneViewModel.DeliveryZoneOldName);
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueDeliveryZoneNameException uniqueDeliveryZoneNameException = aggregatedException as UniqueDeliveryZoneNameException;

						if (uniqueDeliveryZoneNameException != null)
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueDeliveryZoneNameException.ConflictingValue));
					}
				}
			}

			DeliveryZone deliveryZoneFound = _deliveryRepository.GetDeliveryZones(_applicationUserProvider)
																.FirstOrDefault(deliveryZone => string.Equals(deliveryZoneViewModel.DeliveryZoneOldName, deliveryZone.Name, StringComparison.OrdinalIgnoreCase));
			if (deliveryZoneFound != null)
			{
				deliveryZoneViewModel.AvailableAddresses.Clear();
				foreach (KeyValuePair<string, bool> address in deliveryZoneFound.Addresses.Select(address => new KeyValuePair<string, bool>(address, true))
																						  .Concat(Request.Form.Keys.Cast<string>()
																												   .Where(inputName => inputName.StartsWith("checkbox "))
																												   .Select(inputName => new KeyValuePair<string, bool>(inputName.Substring(9), true)))
																						  .Concat(_deliveryRepository.GetUnmappedAddresses().Select(address => new KeyValuePair<string, bool>(address, false))))
					deliveryZoneViewModel.AvailableAddresses.Add(address);
			}
			else
				return EditDeliveryZone(deliveryZoneViewModel.DeliveryZoneName);

			deliveryZoneViewModel.SubmitButtonText = EditDeliveryZoneViewStrings.SubmitButton_DisplayName;
			return View(deliveryZoneViewModel);
		}
		[HttpGet]
		public ActionResult RemoveDeliveryZone()
		{
			return View(_deliveryRepository.GetDeliveryZones(_applicationUserProvider));
		}
		[HttpPost]
		public ActionResult RemoveDeliveryZone(string deliveryZoneName)
		{
			if (deliveryZoneName != null)
				try
				{
					_deliveryRepository.RemoveDeliveryZone(deliveryZoneName);
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueDeliveryZoneNameException uniqueDeliveryZoneNameException = aggregatedException as UniqueDeliveryZoneNameException;

						if (uniqueDeliveryZoneNameException != null)
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueDeliveryZoneNameException.ConflictingValue));
					}
				}

			return RemoveDeliveryZone();
		}

		[HttpGet]
		public ActionResult ScheduleTasks()
		{
			IDictionary<DeliveryZone, IEnumerable<Order>> ordersByDeliveryZone = new Dictionary<DeliveryZone, IEnumerable<Order>>();
			IDictionary<int, Order> remainingOrders = new SortedList<int, Order>();
			foreach (Order order in _orderRepository.GetOrders(OrderState.Pending, _applicationUserProvider, _productProvider))
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

			return RedirectToAction("Planning", "Delivery");
		}

		[ChildActionOnly]
		public ActionResult PendingTasks()
		{
			return View(_deliveryTaskRepository.GetUndergoingDeliveryTasks(_applicationUserProvider, _deliveryRepository, _productProvider, _orderRepository));
		}

		[ChildActionOnly]
		public ActionResult PendingOrders()
		{
			return View(_orderRepository.GetOrders(OrderState.Pending, _applicationUserProvider, _productProvider));
		}

		private IEnumerable<string> _GetUnuesdAddresses()
		{
			return _deliveryRepository.GetUnmappedAddresses()
									  .Except(_orderRepository.GetUsedAddresses());
		}
		private DeliveryZone _GetDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			return new DeliveryZone(deliveryZoneViewModel.DeliveryZoneName,
									Colour.Parse(deliveryZoneViewModel.DeliveryZoneColour),
									Request.Form.Keys.Cast<string>()
													 .Where(inputName => inputName.StartsWith("checkbox "))
													 .Select(inputName => inputName.Substring(9)))
				{
					Assignee = _applicationUserProvider.GetEmployees().FirstOrDefault(employee => string.Equals(employee.EMailAddress, deliveryZoneViewModel.SelectedEmployeeEMailAddress, StringComparison.Ordinal))
				};
		}
		private IEnumerable<int> _Range(int from = 0, int to = 0)
		{
			for (int value = from; value < to; value++)
				yield return value;
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

		private readonly IConstraintSatisfactionSearch<int, Order> _ordersPartitionSearch = new ForwardCheckingSearch<int, Order>();
		private readonly IConstraintSatisfactionSearch<int, OrdersPartition> _disjointOrdersPartitionSearch = new ForwardCheckingSearch<int, OrdersPartition>();
		private readonly IOrderRepository _orderRepository = (IOrderRepository)MvcApplication.DependencyContainer["orderRepository"];
		private readonly IProductProvider _productProvider = (IProductProvider)MvcApplication.DependencyContainer["productRepository"];
		private readonly IDeliveryZoneRepository _deliveryRepository = (IDeliveryZoneRepository)MvcApplication.DependencyContainer["deliveryRepository"];
		private readonly IApplicationUserProvider _applicationUserProvider = (IApplicationUserProvider)MvcApplication.DependencyContainer["applicationUserRepository"];
		private readonly IDeliveryTaskRepository _deliveryTaskRepository = (IDeliveryTaskRepository)MvcApplication.DependencyContainer["deliveryTaskRepository"];

		private sealed class OrdersPartition
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
			public int TotalCapacity
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
}