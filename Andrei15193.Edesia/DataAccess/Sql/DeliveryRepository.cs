using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Sql
{
	public class DeliveryRepository
		: IDeliveryRepository, IDisposable
	{
		public DeliveryRepository(ITranslator<IDataRecord, ApplicationUser> applicationUserTranslator, ITranslator<IDataRecord, Product> productTranslator)
		{
			if (productTranslator == null)
				throw new ArgumentNullException("productTranslator");
			if (applicationUserTranslator == null)
				throw new ArgumentNullException("applicationUserTranslator");

			_productTranslator = productTranslator;
			_applicationUserTranslator = applicationUserTranslator;

			_sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable(string.Format(MvcApplication.AzureConnectionStringFormat, "EdesiaDatabaseConnectionString")));
			_sqlConnection.Open();

			_addOrderCommand.Connection = _sqlConnection;
			_updateOrderCommand.Connection = _sqlConnection;
			_addOrderedProductCommand.Connection = _sqlConnection;
			_getOrdersCommand.Connection = _sqlConnection;
			_getOrdersForRecipientCommand.Connection = _sqlConnection;

			_addStreetCommand.Connection = _sqlConnection;
			_removeStreetCommand.Connection = _sqlConnection;
			_updateStreetCommand.Connection = _sqlConnection;
			_getStreets.Connection = _sqlConnection;
			_getUnmappedStreets.Connection = _sqlConnection;

			_addDeliveryZoneCommand.Connection = _sqlConnection;
			_updateDeliveryZoneCommand.Connection = _sqlConnection;
			_removeDeliveryZoneCommand.Connection = _sqlConnection;
			_getDeliveryZoneCommand.Connection = _sqlConnection;
			_getDeliveryZonesCommand.Connection = _sqlConnection;
			_clearStreetsFromDeliveryZone.Connection = _sqlConnection;

			_addDeliveryTaskCommand.Connection = _sqlConnection;
			_updateDeliveryTaskComand.Connection = _sqlConnection;
			_addScheduledOrderCommand.Connection = _sqlConnection;
			_getDeliveryTasksCommand.Connection = _sqlConnection;
			_getDeliveryTasksForEmployeeCommand.Connection = _sqlConnection;
			_getCanceledOrdersCommand.Connection = _sqlConnection;
			_getCanceledOrdersForEmployeeCommand.Connection = _sqlConnection;
			_getDeliveryTaskByNumberCommand.Connection = _sqlConnection;
		}

		public ITranslator<IDataRecord, ApplicationUser> ApplicationUserTranslator
		{
			get
			{
				return _applicationUserTranslator;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("ApplicationUserTranslator");

				_applicationUserTranslator = value;
			}
		}
		public ITranslator<IDataRecord, Product> ProductTranslator
		{
			get
			{
				return _productTranslator;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("ProductTranslator");

				_productTranslator = value;
			}
		}
		#region IDeliveryRepository Members
		public Order Add(OrderDetails orderDatails)
		{
			_CheckIfDisposed();
			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction())
				{
					_addOrderCommand.Transaction = sqlTransaction;
					_addOrderedProductCommand.Transaction = sqlTransaction;

					_addOrderCommand.Parameters[Database.Orders.DatePlaced].Value = orderDatails.DatePlaced;
					_addOrderCommand.Parameters[Database.Orders.DeliveryStreet].Value = orderDatails.DeliveryAddress.Street;
					_addOrderCommand.Parameters[Database.Orders.DeliveryAddessDetails].Value = orderDatails.DeliveryAddress.Details;
					_addOrderCommand.Parameters[Database.Orders.Recipient].Value = orderDatails.Recipient.EMailAddress;
					_addOrderCommand.ExecuteNonQuery();

					int orderNumber = Convert.ToInt32(_addOrderCommand.Parameters[Database.Orders.Number].Value);
					_addOrderedProductCommand.Parameters[Database.OrderedProducts.OrderNumber].Value = orderNumber;
					foreach (OrderedProduct orderedProduct in orderDatails.OrderedProducts)
					{
						_addOrderedProductCommand.Parameters[Database.OrderedProducts.Product].Value = orderedProduct.Product.Name;
						_addOrderedProductCommand.Parameters[Database.OrderedProducts.Quantity].Value = orderedProduct.Quantity;
						_addOrderedProductCommand.ExecuteNonQuery();
					}

					sqlTransaction.Commit();
					return new Order(orderNumber, orderDatails);
				}
			}
		}
		public void Update(Order order)
		{
			_CheckIfDisposed();
			if (order == null)
				throw new ArgumentNullException("order");

			lock (_sqlConnection)
			{
				_updateOrderCommand.Parameters[Database.Orders.Number].Value = order.Number;
				_updateOrderCommand.Parameters[Database.Orders.State].Value = (int)order.State;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				_updateOrderCommand.ExecuteNonQuery();
			}
		}
		public IEnumerable<Order> GetOrders(params OrderState[] orderStates)
		{
			_CheckIfDisposed();
			if (orderStates == null)
				throw new ArgumentNullException("orderStates");
			if (orderStates.Length == 0)
				throw new ArgumentException("The must be at least one order state!", "orderStates");

			lock (_sqlConnection)
			{
				ICollection<Order> orders = new LinkedList<Order>();
				IDictionary<string, Product> productCache = new SortedDictionary<string, Product>(StringComparer.OrdinalIgnoreCase);
				IDictionary<string, ApplicationUser> applicationUserCache = new SortedDictionary<string, ApplicationUser>(StringComparer.OrdinalIgnoreCase);

				foreach (OrderState orderState in orderStates)
				{
					_getOrdersCommand.Parameters[Database.Orders.State].Value = (int)orderState;

					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					using (IDataReader orderDataReader = _getOrdersCommand.ExecuteReader())
						if (orderDataReader.Read())
						{
							int orderNumber = Convert.ToInt32(orderDataReader[Database.Orders.Number]);
							Order order = _TranslateOrder(applicationUserCache, orderDataReader);

							order.OrderedProducts.Add(new OrderedProduct(_TranslateProduct(productCache, orderDataReader),
																		 Convert.ToInt32(orderDataReader[Database.OrderedProducts.Quantity])));
							orders.Add(order);

							while (orderDataReader.Read())
							{
								if (orderNumber != Convert.ToInt32(orderDataReader[Database.Orders.Number]))
								{
									order = _TranslateOrder(applicationUserCache, orderDataReader);
									orders.Add(order);
									orderNumber = order.Number;
								}
								order.OrderedProducts.Add(new OrderedProduct(_TranslateProduct(productCache, orderDataReader),
																			 Convert.ToInt32(orderDataReader[Database.OrderedProducts.Quantity])));
							}
						}
				}

				return orders;
			}
		}
		public IEnumerable<Order> GetOrders(ApplicationUser recipient, params OrderState[] orderStates)
		{
			_CheckIfDisposed();
			if (recipient == null)
				throw new ArgumentNullException("recipient");
			if (orderStates == null)
				throw new ArgumentNullException("orderStates");
			if (orderStates.Length == 0)
				throw new ArgumentException("Must contain at least one OrderState!", "orderStates");

			lock (_sqlConnection)
			{
				ICollection<Order> orders = new LinkedList<Order>();
				IDictionary<string, Product> productCache = new SortedDictionary<string, Product>(StringComparer.OrdinalIgnoreCase);

				foreach (OrderState orderState in orderStates)
				{
					_getOrdersForRecipientCommand.Parameters[Database.Orders.State].Value = (int)orderState;
					_getOrdersForRecipientCommand.Parameters[Database.Orders.Recipient].Value = recipient.EMailAddress;

					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					using (IDataReader orderDataReader = _getOrdersForRecipientCommand.ExecuteReader())
						if (orderDataReader.Read())
						{
							int orderNumber = Convert.ToInt32(orderDataReader[Database.Orders.Number]);
							Order order = _TranslateOrder(recipient, orderDataReader);

							order.OrderedProducts.Add(new OrderedProduct(_TranslateProduct(productCache, orderDataReader),
																					 Convert.ToInt32(orderDataReader[Database.OrderedProducts.Quantity])));
							orders.Add(order);

							while (orderDataReader.Read())
							{
								if (orderNumber != Convert.ToInt32(orderDataReader[Database.Orders.Number]))
								{
									order = _TranslateOrder(recipient, orderDataReader);
									orders.Add(order);
									orderNumber = order.Number;
								}
								order.OrderedProducts.Add(new OrderedProduct(_TranslateProduct(productCache, orderDataReader),
																			 Convert.ToInt32(orderDataReader[Database.OrderedProducts.Quantity])));
							}
						}
				}

				return orders;
			}
		}

		public void Add(string street)
		{
			_CheckIfDisposed();
			if (street == null)
				throw new ArgumentNullException("street");
			if (string.IsNullOrWhiteSpace(street))
				throw new ArgumentException("Cannot be empty or white space!", "street");

			try
			{
				lock (_sqlConnection)
				{
					_addStreetCommand.Parameters[Database.Streets.Name].Value = street.Trim();

					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					_addStreetCommand.ExecuteNonQuery();
				}
			}
			catch (SqlException sqlException)
			{
				throw new AggregateException(new UniqueStreetException(street.Trim(), sqlException));
			}
		}
		public void Remove(string street)
		{
			_CheckIfDisposed();
			if (street == null)
				throw new ArgumentNullException("street");
			if (string.IsNullOrWhiteSpace(street))
				throw new ArgumentException("Cannot be empty or white space!", "street");

			lock (_sqlConnection)
			{
				_removeStreetCommand.Parameters[Database.Streets.Name].Value = street.Trim();

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				_removeStreetCommand.ExecuteNonQuery();
			}
		}
		public IEnumerable<string> GetStreets()
		{
			_CheckIfDisposed();
			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();

				ICollection<string> streets = new LinkedList<string>();

				using (IDataReader streetDataReader = _getStreets.ExecuteReader())
					while (streetDataReader.Read())
						streets.Add(streetDataReader[Database.Streets.Name].ToString());

				return streets;
			}
		}
		public IEnumerable<string> GetUnmappedStreets()
		{
			_CheckIfDisposed();
			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();

				ICollection<string> streets = new LinkedList<string>();

				using (IDataReader streetDataReader = _getUnmappedStreets.ExecuteReader())
					while (streetDataReader.Read())
						streets.Add(streetDataReader[Database.Streets.Name].ToString());

				return streets;
			}
		}

		public void Add(DeliveryZone deliveryZone)
		{
			_CheckIfDisposed();
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			try
			{
				lock (_sqlConnection)
				{
					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction())
					{
						_addDeliveryZoneCommand.Transaction = sqlTransaction;
						_addDeliveryZoneCommand.Parameters[Database.DeliveryZones.Name].Value = deliveryZone.Name;
						_addDeliveryZoneCommand.Parameters[Database.DeliveryZones.Colour].Value = deliveryZone.Colour.ToString();
						if (deliveryZone.Assignee != null)
							_addDeliveryZoneCommand.Parameters[Database.DeliveryZones.Responsible].Value = deliveryZone.Assignee.EMailAddress;
						else
							_addDeliveryZoneCommand.Parameters[Database.DeliveryZones.Responsible].Value = DBNull.Value;
						_addDeliveryZoneCommand.ExecuteNonQuery();

						_updateStreetCommand.Transaction = sqlTransaction;
						_updateStreetCommand.Parameters[Database.Streets.DeliveryZone].Value = deliveryZone.Name;
						foreach (string deliveryZoneStreet in deliveryZone.Streets)
						{
							_updateStreetCommand.Parameters[Database.Streets.Name].Value = deliveryZoneStreet;
							_updateStreetCommand.ExecuteNonQuery();
						}

						sqlTransaction.Commit();
					}
				}
			}
			catch (SqlException sqlException)
			{
				throw new AggregateException(new UniqueDeliveryZoneNameException(deliveryZone.Name, sqlException));
			}
		}
		public void Update(DeliveryZone deliveryZone, string oldDeliveryZoneName)
		{
			_CheckIfDisposed();
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");
			if (oldDeliveryZoneName == null)
				throw new ArgumentNullException("oldDeliveryZoneName");
			if (string.IsNullOrWhiteSpace(oldDeliveryZoneName))
				throw new ArgumentException("Cannot be empty or white space!", "oldDeliveryZoneName");

			try
			{
				lock (_sqlConnection)
				{
					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction())
					{
						_clearStreetsFromDeliveryZone.Transaction = sqlTransaction;
						_clearStreetsFromDeliveryZone.Parameters[Database.Streets.DeliveryZone].Value = oldDeliveryZoneName;
						_clearStreetsFromDeliveryZone.ExecuteNonQuery();

						_updateDeliveryZoneCommand.Transaction = sqlTransaction;
						_updateDeliveryZoneCommand.Parameters[Database.DeliveryZones.Name].Value = deliveryZone.Name;
						_updateDeliveryZoneCommand.Parameters[Database.DeliveryZones.OldName].Value = oldDeliveryZoneName;
						_updateDeliveryZoneCommand.Parameters[Database.DeliveryZones.Colour].Value = deliveryZone.Colour.ToString();
						if (deliveryZone.Assignee != null)
							_updateDeliveryZoneCommand.Parameters[Database.DeliveryZones.Responsible].Value = deliveryZone.Assignee.EMailAddress;
						else
							_updateDeliveryZoneCommand.Parameters[Database.DeliveryZones.Responsible].Value = DBNull.Value;
						_updateDeliveryZoneCommand.ExecuteNonQuery();

						_updateStreetCommand.Transaction = sqlTransaction;
						_updateStreetCommand.Parameters[Database.Streets.DeliveryZone].Value = deliveryZone.Name;
						foreach (string deliveryZoneStreet in deliveryZone.Streets)
						{
							_updateStreetCommand.Parameters[Database.Streets.Name].Value = deliveryZoneStreet;
							_updateStreetCommand.ExecuteNonQuery();
						}

						sqlTransaction.Commit();
					}
				}
			}
			catch (SqlException sqlException)
			{
				throw new AggregateException(new UniqueDeliveryZoneNameException(deliveryZone.Name, sqlException));
			}
		}
		public void Remove(DeliveryZone deliveryZone)
		{
			_CheckIfDisposed();
			if (deliveryZone == null)
				throw new ArgumentNullException("deliveryZone");

			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction())
				{
					_clearStreetsFromDeliveryZone.Transaction = sqlTransaction;
					_clearStreetsFromDeliveryZone.Parameters[Database.Streets.DeliveryZone].Value = deliveryZone.Name;
					_clearStreetsFromDeliveryZone.ExecuteNonQuery();

					_removeDeliveryZoneCommand.Transaction = sqlTransaction;
					_removeDeliveryZoneCommand.Parameters[Database.DeliveryZones.Name].Value = deliveryZone.Name;
					_removeDeliveryZoneCommand.ExecuteNonQuery();

					sqlTransaction.Commit();
				}
			}
		}
		public DeliveryZone GetDeliveryZone(string name)
		{
			_CheckIfDisposed();
			if (name == null)
				throw new ArgumentNullException("name");
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Cannot be empty or white space!", "name");

			lock (_sqlConnection)
			{
				_getDeliveryZoneCommand.Parameters[Database.DeliveryZones.Name].Value = name;
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (IDataReader deliveryZoneDataReader = _getDeliveryZoneCommand.ExecuteReader())
					if (deliveryZoneDataReader.Read())
					{
						DeliveryZone deliveryZone = _TranslateDeliveryZone(deliveryZoneDataReader);

						while (deliveryZoneDataReader.Read())
							deliveryZone.Streets.Add(deliveryZoneDataReader[Database.DeliveryZones.StreetName].ToString());

						return deliveryZone;
					}
					else
						return null;
			}
		}
		public IEnumerable<DeliveryZone> GetDeliveryZones()
		{
			_CheckIfDisposed();
			lock (_sqlConnection)
			{
				ICollection<DeliveryZone> deliveryZones = new LinkedList<DeliveryZone>();
				IDictionary<string, ApplicationUser> applicationUserCache = new SortedDictionary<string, ApplicationUser>(StringComparer.OrdinalIgnoreCase);

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (IDataReader deliveryZoneDataReader = _getDeliveryZonesCommand.ExecuteReader())
					if (deliveryZoneDataReader.Read())
					{
						DeliveryZone deliveryZone = new DeliveryZone(deliveryZoneDataReader[Database.DeliveryZones.Name].ToString(),
																	 Colour.Parse(deliveryZoneDataReader[Database.DeliveryZones.Colour].ToString()));
						deliveryZones.Add(deliveryZone);

						if (deliveryZoneDataReader[Database.ApplicationUsers.EMail] != DBNull.Value)
							deliveryZone.Assignee = _TranslateApplicationUser(applicationUserCache, deliveryZoneDataReader).TryGetRole<Employee>();
						if (deliveryZoneDataReader[Database.DeliveryZones.StreetName] != DBNull.Value)
							deliveryZone.Streets.Add(deliveryZoneDataReader[Database.DeliveryZones.StreetName].ToString());

						while (deliveryZoneDataReader.Read())
						{
							if (!string.Equals(deliveryZoneDataReader[Database.DeliveryZones.Name].ToString(), deliveryZone.Name, StringComparison.OrdinalIgnoreCase))
							{
								deliveryZone = new DeliveryZone(deliveryZoneDataReader[Database.DeliveryZones.Name].ToString(),
																									 Colour.Parse(deliveryZoneDataReader[Database.DeliveryZones.Colour].ToString()));
								deliveryZones.Add(deliveryZone);

								if (deliveryZoneDataReader[Database.ApplicationUsers.EMail] != DBNull.Value)
									deliveryZone.Assignee = _TranslateApplicationUser(applicationUserCache, deliveryZoneDataReader).TryGetRole<Employee>();
							}
							if (deliveryZoneDataReader[Database.DeliveryZones.StreetName] != DBNull.Value)
								deliveryZone.Streets.Add(deliveryZoneDataReader[Database.DeliveryZones.StreetName].ToString());
						}
					}

				return deliveryZones;
			}
		}

		public DeliveryTask Add(DeliveryTaskDetails deliveryTaskDetails)
		{
			_CheckIfDisposed();
			if (deliveryTaskDetails == null)
				throw new ArgumentNullException("deliveryTaskDetails");

			lock (_sqlConnection)
			{
				int deliveryTaskNumber;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction())
				{
					_addDeliveryTaskCommand.Transaction = sqlTransaction;
					_addDeliveryTaskCommand.Parameters[Database.DeliveryTasks.DateScheduled].Value = deliveryTaskDetails.DateScheduled;
					_addDeliveryTaskCommand.ExecuteNonQuery();
					deliveryTaskNumber = Convert.ToInt32(_addDeliveryTaskCommand.Parameters[Database.DeliveryTasks.Number].Value);

					_updateOrderCommand.Transaction = sqlTransaction;
					_addScheduledOrderCommand.Transaction = sqlTransaction;

					foreach (Order orderToDeliver in deliveryTaskDetails.OrdersToDeliver)
					{
						_updateOrderCommand.Parameters[Database.Orders.Number].Value = orderToDeliver.Number;
						_updateOrderCommand.Parameters[Database.Orders.State].Value = (int)orderToDeliver.State;
						_updateOrderCommand.ExecuteNonQuery();

						_addScheduledOrderCommand.Parameters[Database.ScheduledOrders.DeliveryTask].Value = deliveryTaskNumber;
						_addScheduledOrderCommand.Parameters[Database.ScheduledOrders.OrderNumber].Value = orderToDeliver.Number;
						_addScheduledOrderCommand.ExecuteNonQuery();
					}

					sqlTransaction.Commit();
				}

				return new DeliveryTask(deliveryTaskNumber,
										deliveryTaskDetails.DateScheduled,
										deliveryTaskDetails.DeliveryZone,
										isCancelled: false,
										ordersToDeliver: deliveryTaskDetails.OrdersToDeliver);
			}
		}
		public IEnumerable<DeliveryTask> Add(IEnumerable<DeliveryTaskDetails> deliveryTasksDetails)
		{
			_CheckIfDisposed();
			if (deliveryTasksDetails == null)
				throw new ArgumentNullException("deliveryTasksDetails");

			lock (_sqlConnection)
			{
				ICollection<DeliveryTask> deliveryTasks = new LinkedList<DeliveryTask>();

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction())
				{
					foreach (DeliveryTaskDetails deliveryTaskDetails in deliveryTasksDetails)
					{
						_addDeliveryTaskCommand.Transaction = sqlTransaction;
						_addDeliveryTaskCommand.Parameters[Database.DeliveryTasks.DateScheduled].Value = deliveryTaskDetails.DateScheduled;
						_addDeliveryTaskCommand.ExecuteNonQuery();
						int deliveryTaskNumber = Convert.ToInt32(_addDeliveryTaskCommand.Parameters[Database.DeliveryTasks.Number].Value);

						_updateOrderCommand.Transaction = sqlTransaction;
						_addScheduledOrderCommand.Transaction = sqlTransaction;
						foreach (Order orderToDeliver in deliveryTaskDetails.OrdersToDeliver)
						{
							_updateOrderCommand.Parameters[Database.Orders.Number].Value = orderToDeliver.Number;
							_updateOrderCommand.Parameters[Database.Orders.State].Value = (int)OrderState.Scheduled;
							_updateOrderCommand.ExecuteNonQuery();

							_addScheduledOrderCommand.Parameters[Database.ScheduledOrders.DeliveryTask].Value = deliveryTaskNumber;
							_addScheduledOrderCommand.Parameters[Database.ScheduledOrders.OrderNumber].Value = orderToDeliver.Number;
							_addScheduledOrderCommand.ExecuteNonQuery();
						}

						deliveryTasks.Add(new DeliveryTask(deliveryTaskNumber,
														   deliveryTaskDetails.DateScheduled,
														   deliveryTaskDetails.DeliveryZone,
														   isCancelled: false,
														   ordersToDeliver: deliveryTaskDetails.OrdersToDeliver));
					}

					sqlTransaction.Commit();
					return deliveryTasks;
				}
			}
		}
		public void Update(DeliveryTask deliveryTask)
		{
			_CheckIfDisposed();
			if (deliveryTask == null)
				throw new ArgumentNullException("deliveryTask");

			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction())
				{
					_updateDeliveryTaskComand.Transaction = sqlTransaction;
					_updateDeliveryTaskComand.Parameters[Database.DeliveryTasks.Number].Value = deliveryTask.Number;
					_updateDeliveryTaskComand.Parameters[Database.DeliveryTasks.IsCanceled].Value = deliveryTask.IsCancelled;
					_updateDeliveryTaskComand.ExecuteNonQuery();

					_updateOrderCommand.Transaction = sqlTransaction;
					foreach (Order orderToDeliver in deliveryTask.OrdersToDeliver)
					{
						_updateOrderCommand.Parameters[Database.Orders.Number].Value = orderToDeliver.Number;
						_updateOrderCommand.Parameters[Database.Orders.State].Value = (int)orderToDeliver.State;
						_updateOrderCommand.ExecuteNonQuery();
					}

					sqlTransaction.Commit();
				}
			}
		}
		public DeliveryTask GetDeliveryTask(int task)
		{
			_CheckIfDisposed();

			lock (_sqlConnection)
			{
				IDataReader deliveryTaskDataReader = null;
				IDictionary<string, ApplicationUser> applicationUserCache = new SortedDictionary<string, ApplicationUser>(StringComparer.OrdinalIgnoreCase);
				IDictionary<string, Product> productCache = new SortedDictionary<string, Product>(StringComparer.OrdinalIgnoreCase);

				try
				{
					_getDeliveryTaskByNumberCommand.Parameters[Database.DeliveryTasks.Number].Value = task;
					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					deliveryTaskDataReader = _getDeliveryTaskByNumberCommand.ExecuteReader();

					return _GetDeliveryTasks(applicationUserCache, productCache, deliveryTaskDataReader).FirstOrDefault();
				}
				finally
				{
					if (deliveryTaskDataReader != null)
						deliveryTaskDataReader.Dispose();
				}
			}
		}
		public IEnumerable<DeliveryTask> GetDeliveryTasks(params TaskState[] taskStates)
		{
			_CheckIfDisposed();
			if (taskStates == null)
				throw new ArgumentNullException("taskStates");
			if (taskStates.Length == 0)
				throw new ArgumentException("there must be at least one task state!", "taskStates");

			lock (_sqlConnection)
			{
				IDataReader deliveryTaskDataReader = null;
				ICollection<DeliveryTask> deliveryTasks = new LinkedList<DeliveryTask>();
				IDictionary<string, ApplicationUser> applicationUserCache = new SortedDictionary<string, ApplicationUser>(StringComparer.OrdinalIgnoreCase);
				IDictionary<string, Product> productCache = new SortedDictionary<string, Product>(StringComparer.OrdinalIgnoreCase);

				foreach (TaskState taskState in taskStates)
					try
					{
						OrderState? orderState = _GetOrdersState(taskState);

						if (_sqlConnection.State == ConnectionState.Closed)
							_sqlConnection.Open();
						if (orderState.HasValue)
						{
							_getDeliveryTasksCommand.Parameters[Database.Orders.State].Value = (int)orderState.Value;
							deliveryTaskDataReader = _getDeliveryTasksCommand.ExecuteReader();
						}
						else
							deliveryTaskDataReader = _getCanceledOrdersCommand.ExecuteReader();

						foreach (DeliveryTask deliveryTask in _GetDeliveryTasks(applicationUserCache, productCache, deliveryTaskDataReader))
							deliveryTasks.Add(deliveryTask);
					}
					finally
					{
						if (deliveryTaskDataReader != null)
							deliveryTaskDataReader.Dispose();
					}

				return deliveryTasks;
			}
		}
		public IEnumerable<DeliveryTask> GetDeliveryTasks(Employee employee, params TaskState[] taskStates)
		{
			_CheckIfDisposed();
			if (employee == null)
				throw new ArgumentNullException("employee");
			if (taskStates == null)
				throw new ArgumentNullException("taskStates");
			if (taskStates.Length == 0)
				throw new ArgumentException("there must be at least one task state!", "taskStates");

			lock (_sqlConnection)
			{
				ICollection<DeliveryTask> deliveryTasks = new LinkedList<DeliveryTask>();
				IDictionary<string, Product> productCache = new SortedDictionary<string, Product>(StringComparer.OrdinalIgnoreCase);
				IDictionary<string, ApplicationUser> applicationUserCache = new SortedDictionary<string, ApplicationUser>(StringComparer.OrdinalIgnoreCase);
				IDataReader deliveryTaskDataReader = null;

				foreach (TaskState taskState in taskStates)
					try
					{
						OrderState? orderState = _GetOrdersState(taskState);

						if (_sqlConnection.State == ConnectionState.Closed)
							_sqlConnection.Open();
						if (orderState.HasValue)
						{
							_getDeliveryTasksForEmployeeCommand.Parameters[Database.Orders.State].Value = (int)orderState.Value;
							_getDeliveryTasksForEmployeeCommand.Parameters[Database.ApplicationUsers.EMail].Value = employee.EMailAddress;
							deliveryTaskDataReader = _getDeliveryTasksForEmployeeCommand.ExecuteReader();
						}
						else
						{
							_getCanceledOrdersForEmployeeCommand.Parameters[Database.ApplicationUsers.EMail].Value = employee.EMailAddress;
							deliveryTaskDataReader = _getCanceledOrdersForEmployeeCommand.ExecuteReader();
						}

						foreach (DeliveryTask deliveryTask in _GetDeliveryTasks(employee, applicationUserCache, productCache, deliveryTaskDataReader))
							deliveryTasks.Add(deliveryTask);
					}
					finally
					{
						if (deliveryTaskDataReader != null)
							deliveryTaskDataReader.Dispose();
					}

				return deliveryTasks;
			}
		}
		#endregion
		#region IDisposable Members
		public void Dispose()
		{
			_CheckIfDisposed();

			_isDisposed = true;
			_sqlConnection.Dispose();
			GC.SuppressFinalize(this);
		}
		#endregion

		private void _CheckIfDisposed()
		{
			if (_isDisposed)
				throw new InvalidOperationException("Instance has been disposed!");
		}
		private Order _TranslateOrder(IDictionary<string, ApplicationUser> applicationUserCache, IDataRecord orderDataRecord)
		{
			return _TranslateOrder(_TranslateApplicationUser(applicationUserCache, orderDataRecord), orderDataRecord);
		}
		private Order _TranslateOrder(ApplicationUser recipient, IDataRecord orderDataRecord)
		{
			return new Order(Convert.ToInt32(orderDataRecord[Database.Orders.Number]),
							 (DateTime)orderDataRecord[Database.Orders.DatePlaced],
							 recipient,
							 new DeliveryAddress(orderDataRecord[Database.Orders.DeliveryStreet].ToString(),
												 orderDataRecord[Database.Orders.DeliveryAddessDetails].ToString()),
							 (OrderState)Convert.ToInt32(orderDataRecord[Database.Orders.State]));
		}
		private Product _TranslateProduct(IDictionary<string, Product> productCache, IDataRecord productDataRecord)
		{
			Product product;

			if (!productCache.TryGetValue(productDataRecord[Database.Products.Name].ToString(), out product))
			{
				product = _productTranslator.Translate(productDataRecord);
				productCache.Add(product.Name, product);
			}

			return product;
		}
		private ApplicationUser _TranslateApplicationUser(IDictionary<string, ApplicationUser> applicationUserCache, IDataRecord applicationUserDataRecord)
		{
			ApplicationUser applicationUser;

			if (!applicationUserCache.TryGetValue(applicationUserDataRecord[Database.ApplicationUsers.EMail].ToString(), out applicationUser))
			{
				applicationUser = _applicationUserTranslator.Translate(applicationUserDataRecord);
				applicationUserCache.Add(applicationUser.EMailAddress, applicationUser);
			}

			return applicationUser;
		}
		private DeliveryZone _TranslateDeliveryZone(IDataRecord deliveryZoneDataRecord)
		{
			if (deliveryZoneDataRecord[Database.ApplicationUsers.EMail] != DBNull.Value)
				return _TranslateDeliveryZone(_applicationUserTranslator.Translate(deliveryZoneDataRecord), deliveryZoneDataRecord);
			else
				return _TranslateDeliveryZone(null, deliveryZoneDataRecord);
		}
		private DeliveryZone _TranslateDeliveryZone(ApplicationUser applicationUser, IDataRecord deliveryZoneDataRecord)
		{
			DeliveryZone deliveryZone = new DeliveryZone(deliveryZoneDataRecord[Database.DeliveryZones.Name].ToString(),
														 Colour.Parse(deliveryZoneDataRecord[Database.DeliveryZones.Colour].ToString()));

			if (applicationUser != null)
				deliveryZone.Assignee = applicationUser.TryGetRole<Employee>();

			if (deliveryZoneDataRecord[Database.DeliveryZones.StreetName] != DBNull.Value)
				deliveryZone.Streets.Add(deliveryZoneDataRecord[Database.DeliveryZones.StreetName].ToString());

			return deliveryZone;
		}
		private IEnumerable<DeliveryTask> _GetDeliveryTasks(IDictionary<string, ApplicationUser> applicationUserCache, IDictionary<string, Product> productCache, IDataReader deliveryTaskDataReader)
		{
			PrefixedNameDataRecordProxy employeePrefixedNameDataRecordProxy = new PrefixedNameDataRecordProxy("employee", deliveryTaskDataReader);
			PrefixedNameDataRecordProxy deliveryZonePrefixedNameDataRecordProxy = new PrefixedNameDataRecordProxy("deliveryZone", deliveryTaskDataReader);
			ICollection<DeliveryTask> deliveryTasks = new LinkedList<DeliveryTask>();

			if (deliveryTaskDataReader.Read())
			{
				Order order = _TranslateOrder(applicationUserCache, deliveryTaskDataReader);
				DeliveryTask deliveryTask = new DeliveryTask(Convert.ToInt32(deliveryTaskDataReader[Database.DeliveryTasks.DeliveryTaskNumber]),
															 (DateTime)deliveryTaskDataReader[Database.DeliveryTasks.DateScheduled],
															 _TranslateDeliveryZone(_TranslateApplicationUser(applicationUserCache, employeePrefixedNameDataRecordProxy), deliveryZonePrefixedNameDataRecordProxy),
															 Convert.ToBoolean(deliveryTaskDataReader[Database.DeliveryTasks.IsCanceled]));
				deliveryTasks.Add(deliveryTask);
				deliveryTask.OrdersToDeliver.Add(order);

				order.OrderedProducts.Add(new OrderedProduct(_TranslateProduct(productCache, deliveryTaskDataReader),
															 Convert.ToInt32(deliveryTaskDataReader[Database.OrderedProducts.Quantity])));

				while (deliveryTaskDataReader.Read())
				{
					if (deliveryTask.Number != Convert.ToInt32(deliveryTaskDataReader[Database.DeliveryTasks.DeliveryTaskNumber]))
					{
						deliveryTask = new DeliveryTask(Convert.ToInt32(deliveryTaskDataReader[Database.DeliveryTasks.DeliveryTaskNumber]),
												   (DateTime)deliveryTaskDataReader[Database.DeliveryTasks.DateScheduled],
												   _TranslateDeliveryZone(_TranslateApplicationUser(applicationUserCache, employeePrefixedNameDataRecordProxy), deliveryZonePrefixedNameDataRecordProxy),
												   Convert.ToBoolean(deliveryTaskDataReader[Database.DeliveryTasks.IsCanceled]));
						deliveryTasks.Add(deliveryTask);
					}
					if (order.Number != Convert.ToInt32(deliveryTaskDataReader[Database.Orders.Number]))
					{
						order = _TranslateOrder(applicationUserCache, deliveryTaskDataReader);
						deliveryTask.OrdersToDeliver.Add(order);
					}
					order.OrderedProducts.Add(new OrderedProduct(_TranslateProduct(productCache, deliveryTaskDataReader),
																 Convert.ToInt32(deliveryTaskDataReader[Database.OrderedProducts.Quantity])));
				}
			}

			return deliveryTasks;
		}
		private IEnumerable<DeliveryTask> _GetDeliveryTasks(Employee employee, IDictionary<string, ApplicationUser> applicationUserCache, IDictionary<string, Product> productCache, IDataReader deliveryTaskDataReader)
		{
			DeliveryTask deliveryTask;
			PrefixedNameDataRecordProxy employeePrefixedNameDataRecordProxy = new PrefixedNameDataRecordProxy("employee", deliveryTaskDataReader);
			PrefixedNameDataRecordProxy deliveryZonePrefixedNameDataRecordProxy = new PrefixedNameDataRecordProxy("deliveryZone", deliveryTaskDataReader);

			if (deliveryTaskDataReader.Read())
			{
				Order order = _TranslateOrder(applicationUserCache, deliveryTaskDataReader);

				deliveryTask = new DeliveryTask(Convert.ToInt32(deliveryTaskDataReader[Database.DeliveryTasks.DeliveryTaskNumber]),
												(DateTime)deliveryTaskDataReader[Database.DeliveryTasks.DateScheduled],
												_TranslateDeliveryZone(employee, deliveryZonePrefixedNameDataRecordProxy),
												Convert.ToBoolean(deliveryTaskDataReader[Database.DeliveryTasks.IsCanceled]));
				deliveryTask.OrdersToDeliver.Add(order);

				order.OrderedProducts.Add(new OrderedProduct(_TranslateProduct(productCache, deliveryTaskDataReader),
															 Convert.ToInt32(deliveryTaskDataReader[Database.OrderedProducts.Quantity])));

				while (deliveryTaskDataReader.Read())
				{
					if (deliveryTask.Number != Convert.ToInt32(deliveryTaskDataReader[Database.DeliveryTasks.DeliveryTaskNumber]))
					{
						yield return deliveryTask;
						deliveryTask = new DeliveryTask(Convert.ToInt32(deliveryTaskDataReader[Database.DeliveryTasks.DeliveryTaskNumber]),
												   (DateTime)deliveryTaskDataReader[Database.DeliveryTasks.DateScheduled],
												   _TranslateDeliveryZone(_TranslateApplicationUser(applicationUserCache, employeePrefixedNameDataRecordProxy), deliveryZonePrefixedNameDataRecordProxy),
												   Convert.ToBoolean(deliveryTaskDataReader[Database.DeliveryTasks.IsCanceled]));
					}
					if (order.Number != Convert.ToInt32(deliveryTaskDataReader[Database.Orders.Number]))
					{
						order = _TranslateOrder(applicationUserCache, deliveryTaskDataReader);
						deliveryTask.OrdersToDeliver.Add(order);
					}
					order.OrderedProducts.Add(new OrderedProduct(_TranslateProduct(productCache, deliveryTaskDataReader),
																 Convert.ToInt32(deliveryTaskDataReader[Database.OrderedProducts.Quantity])));
				}

				yield return deliveryTask;
			}
		}

		private OrderState? _GetOrdersState(TaskState taskState)
		{
			switch (taskState)
			{
				case TaskState.Unknown:
					throw new InvalidOperationException("Cannot return delivery tasks in unknown state");
				case TaskState.Scheduled:
					return OrderState.Scheduled;
				case TaskState.InProgress:
					return OrderState.EnRoute;
				case TaskState.Completed:
					return OrderState.Delivered;
				case TaskState.Cancelled:
				default:
					return null;
			}
		}

		private bool _isDisposed = false;
		private ITranslator<IDataRecord, ApplicationUser> _applicationUserTranslator;
		private ITranslator<IDataRecord, Product> _productTranslator;

		private readonly SqlConnection _sqlConnection;
		private readonly SqlCommand _addOrderCommand =
			new SqlCommand
			{
				CommandText = "AddOrder",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.Orders.DatePlaced, SqlDbType.DateTime),
					new SqlParameter(Database.Orders.DeliveryStreet, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.Orders.DeliveryAddessDetails, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.Orders.Recipient, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.Orders.Number, SqlDbType.Int)
					{
						Direction = ParameterDirection.Output
					}
				}
			};
		private readonly SqlCommand _addOrderedProductCommand =
			new SqlCommand
			{
				CommandText = "AddOrderedProduct",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.OrderedProducts.Product, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.OrderedProducts.Quantity, SqlDbType.Int),
					new SqlParameter(Database.OrderedProducts.OrderNumber, SqlDbType.Int)
				}
			};
		private readonly SqlCommand _updateOrderCommand =
			new SqlCommand
			{
				CommandText = "UpdateOrder",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.Orders.Number, SqlDbType.Int),
					new SqlParameter(Database.Orders.State, SqlDbType.Int)
				}
			};
		private readonly SqlCommand _getOrdersCommand =
			new SqlCommand
			{
				CommandText = string.Format("select * from GetOrders(@{0}) order by number",
											Database.Orders.State),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.Orders.State, SqlDbType.Int)
				}
			};
		private readonly SqlCommand _getOrdersForRecipientCommand =
			new SqlCommand
			{
				CommandText = string.Format("select * from GetOrdersForRecipient(@{0}, @{1}) order by number",
											Database.Orders.State,
											Database.Orders.Recipient),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.Orders.State, SqlDbType.Int),
					new SqlParameter(Database.Orders.Recipient, SqlDbType.NVarChar, size: 256)
				}
			};

		private readonly SqlCommand _addStreetCommand =
			new SqlCommand
			{
				CommandText = "AddStreet",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.Streets.Name, SqlDbType.NVarChar, size: 100)
				}
			};
		private readonly SqlCommand _updateStreetCommand =
			new SqlCommand
			{
				CommandText = "UpdateStreet",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.Streets.Name, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.Streets.DeliveryZone, SqlDbType.NVarChar, size: 100)
				}
			};
		private readonly SqlCommand _removeStreetCommand =
			new SqlCommand
			{
				CommandText = "RemoveStreet",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.Streets.Name, SqlDbType.NVarChar, size: 100)
				}
			};
		private readonly SqlCommand _getStreets =
			new SqlCommand
			{
				CommandText = "select * from ActualStreets order by name",
				CommandType = CommandType.Text
			};
		private readonly SqlCommand _getUnmappedStreets =
			new SqlCommand
			{
				CommandText = "select * from UnmappedStreets order by name",
				CommandType = CommandType.Text
			};

		private readonly SqlCommand _addDeliveryZoneCommand =
			new SqlCommand
			{
				CommandText = "AddDeliveryZone",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.DeliveryZones.Name, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.DeliveryZones.Colour, SqlDbType.VarChar, size: 7),
					new SqlParameter(Database.DeliveryZones.Responsible, SqlDbType.NVarChar, size: 256)
				}
			};
		private readonly SqlCommand _updateDeliveryZoneCommand =
			new SqlCommand
			{
				CommandText = "UpdateDeliveryZone",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.DeliveryZones.Name, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.DeliveryZones.OldName, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.DeliveryZones.Colour, SqlDbType.VarChar, size: 7),
					new SqlParameter(Database.DeliveryZones.Responsible, SqlDbType.NVarChar, size: 256)
				}
			};
		private readonly SqlCommand _removeDeliveryZoneCommand =
			new SqlCommand
			{
				CommandText = "RemoveDeliveryZone",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.DeliveryZones.Name, SqlDbType.NVarChar, size: 100)
				}
			};
		private readonly SqlCommand _getDeliveryZoneCommand =
			new SqlCommand
			{
				CommandText = string.Format("select * from GetDeliveryZone(@{0})",
											Database.DeliveryZones.Name),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.DeliveryZones.Name, SqlDbType.NVarChar, size: 100)
				}
			};
		private readonly SqlCommand _getDeliveryZonesCommand =
			new SqlCommand
			{
				CommandText = "select * from ActualDeliveryZones order by name",
				CommandType = CommandType.Text
			};
		private readonly SqlCommand _clearStreetsFromDeliveryZone =
			new SqlCommand
			{
				CommandText = "ClearStreetsFromDeliveryZone",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.Streets.DeliveryZone, SqlDbType.NVarChar, size: 100)
				}
			};

		private readonly SqlCommand _addDeliveryTaskCommand =
			new SqlCommand
			{
				CommandText = "AddDeliveryTask",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.DeliveryTasks.DateScheduled, SqlDbType.DateTime),
					new SqlParameter(Database.DeliveryTasks.Number, SqlDbType.Int)
					{
						Direction = ParameterDirection.Output
					}
				}
			};
		private readonly SqlCommand _updateDeliveryTaskComand =
			new SqlCommand
			{
				CommandText = "UpdateDeliveryTask",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.DeliveryTasks.Number, SqlDbType.Int),
					new SqlParameter(Database.DeliveryTasks.IsCanceled, SqlDbType.Bit)
				}
			};
		private readonly SqlCommand _addScheduledOrderCommand =
			new SqlCommand
			{
				CommandText = "AddScheduledOrder",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ScheduledOrders.DeliveryTask, SqlDbType.Int),
					new SqlParameter(Database.ScheduledOrders.OrderNumber, SqlDbType.Int)
				}
			};
		private readonly SqlCommand _getDeliveryTasksCommand =
			new SqlCommand
			{
				CommandText = string.Format("select * from GetDeliveryTaskHavingOrdersInState(@{0}) order by deliveryTaskNumber, number",
											Database.Orders.State),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.Orders.State, SqlDbType.Int)
				}
			};
		private readonly SqlCommand _getDeliveryTasksForEmployeeCommand =
			new SqlCommand
			{
				CommandText = string.Format("select * from GetDeliveryTaskHavingOrdersInStateForEmployee(@{0}, @{1}) order by deliveryTaskNumber, number",
											Database.Orders.State,
											Database.ApplicationUsers.EMail),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.Orders.State, SqlDbType.Int),
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256)
				}
			};
		private readonly SqlCommand _getCanceledOrdersCommand =
			new SqlCommand
			{
				CommandText = "select * from CanceledDeliveryTasks order by deliveryTaskNumber, number",
				CommandType = CommandType.Text
			};
		private readonly SqlCommand _getCanceledOrdersForEmployeeCommand =
			new SqlCommand
			{
				CommandText = string.Format("select * from GetCanceledDeliveryTasksForEmployee(@{0}) order by deliveryTaskNumber, number",
											Database.ApplicationUsers.EMail),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 100)
				}
			};
		private readonly SqlCommand _getDeliveryTaskByNumberCommand =
			new SqlCommand
			{
				CommandText = string.Format("select * from GetDeliveryTaskByNumber(@{0}) order by number",
											Database.DeliveryTasks.Number),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.DeliveryTasks.Number, SqlDbType.Int)
				}
			};

		private sealed class PrefixedNameDataRecordProxy
			: IDataRecord
		{
			public PrefixedNameDataRecordProxy(string prefix, IDataRecord dataRecord)
			{
				if (prefix == null)
					throw new ArgumentNullException("prefix");
				if (string.IsNullOrWhiteSpace(prefix))
					throw new ArgumentException("Cannot be empty or white space", "prefix");

				_prefix = prefix;
				_dataRecord = dataRecord;
			}

			public string Prefix
			{
				get
				{
					return _prefix;
				}
				set
				{
					if (value == null)
						throw new ArgumentNullException("Prefix");
					if (string.IsNullOrWhiteSpace(value))
						throw new ArgumentException("Cannot be empty or white space", "Prefix");

					_prefix = value;
				}
			}
			#region IDataRecord Members
			public int FieldCount
			{
				get
				{
					return _dataRecord.FieldCount;
				}
			}
			public bool GetBoolean(int i)
			{
				return _dataRecord.GetBoolean(i);
			}
			public byte GetByte(int i)
			{
				return _dataRecord.GetByte(i);
			}
			public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
			{
				return _dataRecord.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
			}
			public char GetChar(int i)
			{
				return _dataRecord.GetChar(i);
			}
			public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
			{
				return _dataRecord.GetChars(i, fieldoffset, buffer, bufferoffset, length);
			}
			public IDataReader GetData(int i)
			{
				return _dataRecord.GetData(i);
			}
			public string GetDataTypeName(int i)
			{
				return _dataRecord.GetDataTypeName(i);
			}
			public DateTime GetDateTime(int i)
			{
				return _dataRecord.GetDateTime(i);
			}
			public decimal GetDecimal(int i)
			{
				return _dataRecord.GetDecimal(i);
			}
			public double GetDouble(int i)
			{
				return _dataRecord.GetDouble(i);
			}
			public Type GetFieldType(int i)
			{
				return _dataRecord.GetFieldType(i);
			}
			public float GetFloat(int i)
			{
				return _dataRecord.GetFloat(i);
			}
			public Guid GetGuid(int i)
			{
				return _dataRecord.GetGuid(i);
			}
			public short GetInt16(int i)
			{
				return _dataRecord.GetInt16(i);
			}
			public int GetInt32(int i)
			{
				return _dataRecord.GetInt32(i);
			}
			public long GetInt64(int i)
			{
				return _dataRecord.GetInt64(i);
			}
			public string GetName(int i)
			{
				return _dataRecord.GetName(i);
			}
			public int GetOrdinal(string name)
			{
				return _dataRecord.GetOrdinal(_prefix + name);
			}
			public string GetString(int i)
			{
				return _dataRecord.GetString(i);
			}
			public object GetValue(int i)
			{
				return _dataRecord.GetValue(i);
			}
			public int GetValues(object[] values)
			{
				return _dataRecord.GetValues(values);
			}
			public bool IsDBNull(int i)
			{
				return _dataRecord.IsDBNull(i);
			}
			public object this[string name]
			{
				get
				{
					return _dataRecord[_prefix + name];
				}
			}
			public object this[int i]
			{
				get
				{
					return _dataRecord[i];
				}
			}
			#endregion

			private string _prefix;
			private readonly IDataRecord _dataRecord;
		}
	}
}