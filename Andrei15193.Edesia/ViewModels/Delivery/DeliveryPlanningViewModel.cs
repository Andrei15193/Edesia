using System;
namespace Andrei15193.Edesia.ViewModels.Delivery
{
	public class DeliveryPlanningViewModel
	{
		public DeliveryPlanningViewModel(DeliveryZonesViewModel deliveryZonesViewModel)
		{
			if (deliveryZonesViewModel == null)
				throw new ArgumentNullException("deliveryZonesViewModel");

			_deliveryZonesViewModel = deliveryZonesViewModel;
		}

		public DeliveryZonesViewModel DeliveryZonesViewModel
		{
			get
			{
				return _deliveryZonesViewModel;
			}
		}

		private readonly DeliveryZonesViewModel _deliveryZonesViewModel;
	}
}