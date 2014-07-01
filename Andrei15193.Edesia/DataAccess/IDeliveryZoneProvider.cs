using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	[Obsolete]
	public interface IDeliveryZoneProvider
	{
		DeliveryZone GetDeliveryZone(IApplicationUserProvider applicationUserProvider, string deliveryZoneName, DateTime version);
	
		IEnumerable<string> GetUnmappedStreets();
		IEnumerable<DeliveryZone> GetDeliveryZones(IApplicationUserProvider applicationUserProvider);
		IEnumerable<DeliveryZone> GetDeliveryZones(Employee employee);
		
		IEnumerable<string> GetStreets();
	}
}