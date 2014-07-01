using System;
namespace Andrei15193.Edesia.Models
{
	public struct DeliveryAddress
	{
		public DeliveryAddress(string street, string details)
		{
			if (street == null)
				throw new ArgumentNullException("street");
			if (string.IsNullOrWhiteSpace(street))
				throw new ArgumentException("Cannot be empty or whitespace!", "street");

			if (details == null)
				throw new ArgumentNullException("details");
			if (string.IsNullOrWhiteSpace(details))
				throw new ArgumentException("Cannot be empty or whitespace!", "details");

			_street = street;
			_details = details;
		}

		public string Street
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

		private readonly string _street;
		private readonly string _details;
	}
}