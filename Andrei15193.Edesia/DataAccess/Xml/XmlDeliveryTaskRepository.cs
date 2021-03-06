﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class XmlDeliveryTaskRepository
		: IDeliveryTaskRepository
	{
		public XmlDeliveryTaskRepository(string xmlDocumentFileName, XmlDocumentProvider xmlDocumentProvider)
		{
			if (xmlDocumentFileName == null)
				throw new ArgumentNullException("xmlDocumentFileName");
			if (string.IsNullOrEmpty(xmlDocumentFileName) || string.IsNullOrWhiteSpace(xmlDocumentFileName))
				throw new ArgumentException("Cannot be empty or whitespace!", "xmlDocumentFileName");
			if (xmlDocumentProvider == null)
				throw new ArgumentNullException("xmlDocumentProvider");

			_xmlDocumentFileName = xmlDocumentFileName;
			_xmlDocumentProvider = xmlDocumentProvider;
			_xmlDocumentSchemaSet = new XmlSchemaSet();
			_xmlDocumentSchemaSet.Add("http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd", "http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd");
		}

		#region IDeliveryTaskRepository Members
		public IEnumerable<DeliveryTask> AddDeliveryTasks(IEnumerable<DeliveryTaskDetails> deliveryTasksDetails)
		{
			if (deliveryTasksDetails == null)
				throw new ArgumentNullException("deliveryTasksDetails");

			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				ICollection<DeliveryTask> deliveryTasks = new LinkedList<DeliveryTask>();

				foreach (DeliveryTaskDetails deliveryTaskDetails in deliveryTasksDetails.Where(deliveryTaskDetails => (deliveryTaskDetails.DeliveryZone.Assignee != null)))
				{
					DeliveryTask deliveryTask = new DeliveryTask(xmlTransaction.XmlDocument.Root.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}DeliveryTask").Count() + 1,
																 deliveryTaskDetails.DateScheduled,
																 deliveryTaskDetails.DeliveryZone,
																 false,
																 deliveryTaskDetails.OrdersToDeliver);

					xmlTransaction.XmlDocument
								  .Root
								  .Add(new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}DeliveryTask",
													new XAttribute("TaskNumber", deliveryTask.Number),
													new XAttribute("DateScheduled", deliveryTask.DateScheduled.ToString(MvcApplication.DateTimeSerializationFormat)),
													new XAttribute("DeliveryZone", deliveryTask.DeliveryZone.Name),
													deliveryTask.OrdersToDeliver.Select(orderToDeliver => new XElement("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}OrderToDeliver", orderToDeliver.Number))));

					deliveryTasks.Add(deliveryTask);
				}

				xmlTransaction.Commit(newVersion: false);
				return deliveryTasks;
			}
		}
		public IEnumerable<DeliveryTask> AddDeliveryTasks(params DeliveryTaskDetails[] deliveryTasksDetails)
		{
			return AddDeliveryTasks((IEnumerable<DeliveryTaskDetails>)deliveryTasksDetails);
		}

		public DeliveryTask GetDeliveryTask(int taskNumber, IApplicationUserProvider applicationUserProvider, IDeliveryZoneProvider deliveryZoneProvider, IProductProvider productProvider, IOrderProvider orderProvider)
		{
			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement deliveryTaskXElement = xmlTransaction.XmlDocument
															  .Root
															  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}DeliveryTask")
															  .FirstOrDefault(deliveryTaskXmlElement => taskNumber == int.Parse(deliveryTaskXmlElement.Attribute("TaskNumber").Value));

				if (deliveryTaskXElement == null)
					return null;

				return _GetDeliveryTask(deliveryTaskXElement, applicationUserProvider, deliveryZoneProvider, productProvider, orderProvider);
			}
		}
		public IEnumerable<DeliveryTask> GetDeliveryTasks(IApplicationUserProvider applicationUserProvider, IDeliveryZoneProvider deliveryZoneProvider, IProductProvider productProvider, IOrderProvider orderProvider, params TaskState[] taskStates)
		{
			if (applicationUserProvider == null)
				throw new ArgumentNullException("applicationUserProvider");

			if (deliveryZoneProvider == null)
				throw new ArgumentNullException("deliveryZoneProvider");

			if (orderProvider == null)
				throw new ArgumentNullException("orderProvider");

			if (taskStates == null)
				throw new ArgumentNullException("taskStates");
			if (taskStates.Length == 0)
				throw new ArgumentException("Cannot be empty!", "taskStates");

			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
				return xmlTransaction.XmlDocument
									 .Root
									 .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}DeliveryTask")
									 .Select(deliveryTaskXElement => _GetDeliveryTask(deliveryTaskXElement, applicationUserProvider, deliveryZoneProvider, productProvider, orderProvider))
									 .Where(deliveryTask => taskStates.Contains(deliveryTask.State))
									 .OrderBy(deliveryTask => taskStates.TakeWhile(taskState => deliveryTask.State != taskState).Count());
		}
		public IEnumerable<DeliveryTask> GetDeliveryTasks(Employee employee, IApplicationUserProvider applicationUserProvider, IDeliveryZoneProvider deliveryZoneProvider, IProductProvider productProvider, IOrderProvider orderProvider, params TaskState[] taskStates)
		{
			if (employee == null)
				throw new ArgumentNullException("employee");

			if (deliveryZoneProvider == null)
				throw new ArgumentNullException("deliveryZoneProvider");

			if (orderProvider == null)
				throw new ArgumentNullException("orderProvider");

			if (taskStates == null)
				throw new ArgumentNullException("taskStates");
			if (taskStates.Length == 0)
				throw new ArgumentException("Cannot be empty!", "taskStates");

			using (ISharedXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginSharedTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				ICollection<DeliveryTask> deliveryTasks = new List<DeliveryTask>();
				IDictionary<string, DeliveryZone> deliveryZones = new SortedList<string, DeliveryZone>();

				foreach (DeliveryZone deliveryZone in deliveryZoneProvider.GetDeliveryZones(employee))
					deliveryZones.Add(deliveryZone.Name, deliveryZone);

				foreach (XElement deliveryTaskXElement in xmlTransaction.XmlDocument
																		.Root
																		.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}DeliveryTask"))
				{
					DeliveryZone deliveryZone;
					if (deliveryZones.TryGetValue(deliveryTaskXElement.Attribute("DeliveryZone").Value, out deliveryZone))
					{
						DeliveryTask deliveryTask = _GetDeliveryTask(deliveryTaskXElement, deliveryZone, applicationUserProvider, productProvider, orderProvider);
						if (taskStates.Contains(deliveryTask.State))
							deliveryTasks.Add(deliveryTask);
					}
				}

				return deliveryTasks;
			}
		}

		public void CancelTask(int deliveryTaskNumber)
		{
			using (IExclusiveXmlTransaction xmlTransaction = _xmlDocumentProvider.BeginExclusiveTransaction(_xmlDocumentFileName, _xmlDocumentSchemaSet))
			{
				XElement deliveryTaskXElement = xmlTransaction.XmlDocument
															  .Root
															  .Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}DeliveryTask")
															  .FirstOrDefault(deliveryTaskXmlElement => (deliveryTaskNumber == int.Parse(deliveryTaskXmlElement.Attribute("TaskNumber").Value)));

				if (deliveryTaskXElement != null && deliveryTaskXElement.Attribute("Cancelled") == null)
				{
					deliveryTaskXElement.Add(new XAttribute("Cancelled", "Cancelled"));
					xmlTransaction.Commit(newVersion: false);
				}
			}
		}
		#endregion
		public string XmlDocumentFileName
		{
			get
			{
				return _xmlDocumentFileName;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("XmlDocumentFileName");
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Cannot be empty or whitespace!", "XmlDocumentFileName");

				_xmlDocumentFileName = value;
			}
		}
		public XmlDocumentProvider XmlDocumentProvider
		{
			get
			{
				return _xmlDocumentProvider;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("XmlDocumentProvider");

				_xmlDocumentProvider = value;
			}
		}

		private DeliveryTask _GetDeliveryTask(XElement deliveryTaskXElement, IApplicationUserProvider applicationUserProvider, IDeliveryZoneProvider deliveryZoneProvider, IProductProvider productProvider, IOrderProvider orderProvider)
		{
			DateTime scheduleTime = DateTime.ParseExact(deliveryTaskXElement.Attribute("DateScheduled").Value, MvcApplication.DateTimeSerializationFormat, null);

			return new DeliveryTask(int.Parse(deliveryTaskXElement.Attribute("TaskNumber").Value),
									scheduleTime,
									deliveryZoneProvider.GetDeliveryZone(applicationUserProvider, deliveryTaskXElement.Attribute("DeliveryZone").Value, scheduleTime),
									deliveryTaskXElement.Attribute("Cancelled") != null,
									deliveryTaskXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}OrderToDeliver")
														.Select(orderToDeliverXElement => orderProvider.GetOrder(applicationUserProvider, productProvider, int.Parse(orderToDeliverXElement.Value))));
		}

		private DeliveryTask _GetDeliveryTask(XElement deliveryTaskXElement, DeliveryZone deliveryZone, IApplicationUserProvider applicationUserProvider, IProductProvider productProvider, IOrderProvider orderProvider)
		{
			return new DeliveryTask(int.Parse(deliveryTaskXElement.Attribute("TaskNumber").Value),
									DateTime.ParseExact(deliveryTaskXElement.Attribute("DateScheduled").Value, MvcApplication.DateTimeSerializationFormat, null),
									deliveryZone,
									deliveryTaskXElement.Attribute("Cancelled") != null,
									deliveryTaskXElement.Elements("{http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryTask.xsd}OrderToDeliver")
														.Select(orderToDeliverXElement => orderProvider.GetOrder(applicationUserProvider, productProvider, int.Parse(orderToDeliverXElement.Value))));
		}

		private string _xmlDocumentFileName;
		private XmlDocumentProvider _xmlDocumentProvider;
		private readonly XmlSchemaSet _xmlDocumentSchemaSet;
	}
}