using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IDeliveryRepository
	{
		IEnumerable<string> GetUnmappedStreets();
		IEnumerable<DeliveryZone> GetDeliveryZones();

		void AddStreet(string streetName);
		void RemoveStreet(string streetName);

		void AddDeliveryZone(DeliveryZone deliveryZone);
		void UpdateDeliveryZone(DeliveryZone deliveryZone, string deliveryZoneOldName);
		void RemoveDeliveryZone(string deliveryZoneName);
	}
}