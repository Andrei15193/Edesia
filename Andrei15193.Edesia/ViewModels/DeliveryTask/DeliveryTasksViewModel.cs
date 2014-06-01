using System;
using System.Collections;
using System.Collections.Generic;
namespace Andrei15193.Edesia.ViewModels.DeliveryTask
{
	public class DeliveryTasksViewModel
		: IEnumerable<Models.DeliveryTask>
	{
		public DeliveryTasksViewModel(string viewTitle, int expandedDeliveryTaskCount, IEnumerable<Models.DeliveryTask> deliveryTasks)
		{
			if (viewTitle == null)
				throw new ArgumentNullException("viewTitle");
			if (deliveryTasks == null)
				throw new ArgumentNullException("deliveryTasks");

			_viewTitle = viewTitle;
			_expandedDeliveryTaskCount = expandedDeliveryTaskCount;
			_deliveryTasks = deliveryTasks;
		}

		#region IEnumerable<DeliveryTask> Members
		public IEnumerator<Models.DeliveryTask> GetEnumerator()
		{
			return _deliveryTasks.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		public string ViewTitle
		{
			get
			{
				return _viewTitle;
			}
		}
		public int ExpandedDeliveryTaskCount
		{
			get
			{
				return _expandedDeliveryTaskCount;
			}
		}

		private readonly string _viewTitle;
		private readonly int _expandedDeliveryTaskCount;
		private readonly IEnumerable<Models.DeliveryTask> _deliveryTasks;
	}
}