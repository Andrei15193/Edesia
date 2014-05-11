using System;
using System.Collections.Generic;
using System.Linq;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.ViewModels.Delivery
{
	public class DeliveryZonesViewModel
	{
		public DeliveryZonesViewModel(IEnumerable<string> unmappedStreets, IEnumerable<DeliveryZone> deliveryZones, IEnumerable<string> unusedStreets)
		{
			if (unmappedStreets == null)
				throw new ArgumentNullException("unmappedStreets");
			if (deliveryZones == null)
				throw new ArgumentNullException("deliveryZones");
			if (unusedStreets == null)
				throw new ArgumentNullException("unusedStreets");

			_unmappedSteets = unmappedStreets.Where(unmappedStreet => unmappedStreet != null && !string.IsNullOrEmpty(unmappedStreet) && !string.IsNullOrWhiteSpace(unmappedStreet));
			_deliveryZones = deliveryZones.Where(deliveryZone => deliveryZone != null);
			_unusedStreets = unusedStreets;
		}

		public IEnumerable<string> UnmappedStreets
		{
			get
			{
				return _unmappedSteets;
			}
		}
		public IEnumerable<DeliveryZone> DeliveryZones
		{
			get
			{
				return _deliveryZones;
			}
		}
		public IEnumerable<string> UnusedStreets
		{
			get
			{
				return _unusedStreets;
			}
		}

		private readonly IEnumerable<string> _unmappedSteets;
		private readonly IEnumerable<DeliveryZone> _deliveryZones;
		private readonly IEnumerable<string> _unusedStreets;
	}
}