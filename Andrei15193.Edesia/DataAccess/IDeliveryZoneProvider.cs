using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IDeliveryZoneProvider
	{
		DeliveryZone GetDeliveryZone(IApplicationUserProvider applicationUserProvider, string deliveryZoneName, DateTime version);
		IEnumerable<string> GetUnmappedAddresses();
		IEnumerable<DeliveryZone> GetDeliveryZones(IApplicationUserProvider applicationUserProvider);
		IEnumerable<string> GetAddresses();
	}
}