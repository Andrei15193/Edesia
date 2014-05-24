using System;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IDeliveryZoneProvider
	{
		DeliveryZone GetDeliveryZone(IApplicationUserProvider applicationUserProvider, string deliveryZoneName, DateTime version);
	}
}