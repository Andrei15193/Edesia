namespace Andrei15193.Edesia.DataAccess.Sql
{
	internal static class Database
	{
		internal static class ApplicationUsers
		{
			internal const string EMail = "eMail";
			internal const string PasswordHash = "passwordHash";
			internal const string AuthenticationToken = "authenticationToken";
			internal const string RegistrationToken = "registrationToken";
			internal const string FirstName = "firstName";
			internal const string LastName = "lastName";
			internal const string TransportCapacity = "transportCapacity";
			internal const string IsAdministrator = "isAdministrator";
			internal const string DateRegistered = "dateRegistered";
		}

		internal static class Products
		{
			internal const string Name = "name";
			internal const string Price = "price";
			internal const string Capacity = "capacity";
			internal const string ImageLocation = "imageLocation";
		}

		internal static class ShoppingCarts
		{
			internal const string Owner = "owner";
			internal const string Product = "product";
			internal const string Quantity = "quantity";
		}

		internal static class DeliveryZones
		{
			internal const string Name = "name";
			internal const string OldName = "oldName";
			internal const string Colour = "colour";
			internal const string Responsible = "responsible";
			internal const string StreetName = "streetName";
		}

		internal static class Streets
		{
			internal const string Name = "name";
			internal const string DeliveryZone = "deliveryZone";
			internal const string IsUsed = "isUsed";
		}

		internal static class Orders
		{
			internal const string Number = "number";
			internal const string DatePlaced = "datePlaced";
			internal const string DeliveryStreet = "deliveryStreet";
			internal const string DeliveryAddessDetails = "deliveryAddessDetails";
			internal const string Recipient = "recipient";
			internal const string State = "state";
		}

		internal static class OrderedProducts
		{
			internal const string Product = "product";
			internal const string OrderNumber = "orderNumber";
			internal const string Quantity = "quantity";
		}

		internal static class DeliveryTasks
		{
			internal const string Number = "number";
			internal const string DeliveryTaskNumber = "deliveryTaskNumber";
			internal const string DateScheduled = "dateScheduled";
			internal const string IsCanceled = "isCanceled";
			internal const string Employee = "employee";
		}

		internal static class ScheduledOrders
		{
			internal const string DeliveryTask = "deliveryTask";
			internal const string OrderNumber = "orderNumber";
		}
	}
}