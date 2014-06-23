using System;
namespace Andrei15193.Edesia.Models
{
	public struct DeliveryAddress
	{
		public DeliveryAddress(Street street, string details)
		{
			if (street == null)
				throw new ArgumentNullException("street");

			if (details == null)
				throw new ArgumentNullException("details");
			if (string.IsNullOrWhiteSpace(details))
				throw new ArgumentException("Cannot be empty or whitespace!", "street");

			_street = street;
			_details = details;
		}

		public Street Street
		{
			get
			{
				return _street;
			}
		}
		public string Details
		{
			get
			{
				return _details;
			}
		}

		private readonly Street _street;
		private readonly string _details;
	}
}