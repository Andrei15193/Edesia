﻿using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IDeliveryZoneRepository
		: IDeliveryZoneProvider
	{
		void AddAddress(string addressName);
		void RemoveAddress(string addressName);

		void AddDeliveryZone(DeliveryZone deliveryZone);
		void UpdateDeliveryZone(DeliveryZone deliveryZone, string deliveryZoneOldName);
		void RemoveDeliveryZone(string deliveryZoneName);
	}
}