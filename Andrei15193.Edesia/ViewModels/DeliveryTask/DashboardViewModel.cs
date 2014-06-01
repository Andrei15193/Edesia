using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.ViewModels.DeliveryTask
{
	public class DashboardViewModel
	{
		public DashboardViewModel(IEnumerable<Models.DeliveryTask> deliveryTasks)
		{
			if (deliveryTasks == null)
				throw new ArgumentNullException("deliveryTasks");

			IDictionary<TaskState, IEnumerable<Models.DeliveryTask>> deliveryTasksByState = new SortedList<TaskState, IEnumerable<Models.DeliveryTask>>(capacity: 5);
			foreach (Models.DeliveryTask deliveryTask in deliveryTasks)
			{
				IEnumerable<Models.DeliveryTask> deliveryTaskCollection;

				if (deliveryTasksByState.TryGetValue(deliveryTask.State, out deliveryTaskCollection))
					((ICollection<Models.DeliveryTask>)deliveryTaskCollection).Add(deliveryTask);
				else
					deliveryTasksByState.Add(deliveryTask.State, new List<Models.DeliveryTask> { deliveryTask });
			}

			_deliveryTasksByState = new ReadOnlyDictionary<TaskState, IEnumerable<Models.DeliveryTask>>(deliveryTasksByState);
		}

		public IEnumerable<Models.DeliveryTask> this[TaskState taskState]
		{
			get
			{
				IEnumerable<Models.DeliveryTask> deliveryTasks;

				if (_deliveryTasksByState.TryGetValue(taskState, out deliveryTasks))
					return deliveryTasks;
				else
					return MvcApplication.GetEmptyAray<Models.DeliveryTask>();
			}
		}

		private readonly IReadOnlyDictionary<TaskState, IEnumerable<Models.DeliveryTask>> _deliveryTasksByState;
	}
}