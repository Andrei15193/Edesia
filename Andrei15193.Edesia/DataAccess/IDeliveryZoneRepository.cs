using System;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	[Obsolete]
	public interface IDeliveryZoneRepository
		: IDeliveryZoneProvider
	{
		void AddStreet(string street);
		void RemoveStreet(string street);

		void AddDeliveryZone(DeliveryZone deliveryZone);
		void UpdateDeliveryZone(DeliveryZone deliveryZone, string deliveryZoneOldName);
		void RemoveDeliveryZone(string deliveryZoneName);
	}
}