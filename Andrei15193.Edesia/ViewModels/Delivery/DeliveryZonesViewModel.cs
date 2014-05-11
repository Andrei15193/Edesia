using System;
using System.Collections.Generic;
using System.Linq;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.ViewModels.Delivery
{
	public class DeliveryZonesViewModel
	{
		public DeliveryZonesViewModel(IEnumerable<string> unmappedAddresses, IEnumerable<DeliveryZone> deliveryZones, IEnumerable<string> unusedAddresses)
		{
			if (unmappedAddresses == null)
				throw new ArgumentNullException("unmappedAddresses");
			if (deliveryZones == null)
				throw new ArgumentNullException("deliveryZones");
			if (unusedAddresses == null)
				throw new ArgumentNullException("unusedAddress");

			_unmappedAddresses = unmappedAddresses.Where(unmappedAddress => unmappedAddress != null && !string.IsNullOrEmpty(unmappedAddress) && !string.IsNullOrWhiteSpace(unmappedAddress)).OrderBy(address => address);
			_deliveryZones = deliveryZones.Where(deliveryZone => deliveryZone != null).OrderBy(deliveryZone => deliveryZone.Name);
			_unusedAddresses = unusedAddresses.OrderBy(unusedAddress => unusedAddress);
		}

		public IEnumerable<string> UnmappedAddresses
		{
			get
			{
				return _unmappedAddresses;
			}
		}
		public IEnumerable<DeliveryZone> DeliveryZones
		{
			get
			{
				return _deliveryZones;
			}
		}
		public IEnumerable<string> UnusedAddresses
		{
			get
			{
				return _unusedAddresses;
			}
		}

		private readonly IEnumerable<string> _unmappedAddresses;
		private readonly IEnumerable<DeliveryZone> _deliveryZones;
		private readonly IEnumerable<string> _unusedAddresses;
	}
}