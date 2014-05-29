using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models.Collections;
namespace Andrei15193.Edesia.Models
{
	public sealed class OrderDetails
	{
		public OrderDetails(ApplicationUser recipient, string deliveryStreet, string deliveryAddressDetails)
		{
			if (recipient == null)
				throw new ArgumentNullException("recipient");

			if (deliveryStreet == null)
				throw new ArgumentNullException("deliveryStreet");
			if (string.IsNullOrWhiteSpace(deliveryStreet))
				throw new ArgumentException("Cannot be empty or whitespace!", "deliveryStreet");

			if (deliveryAddressDetails == null)
				throw new ArgumentNullException("deliveryAddressDetails");
			if (string.IsNullOrWhiteSpace(deliveryAddressDetails))
				throw new ArgumentException("Cannot be empty or whitespace!", "deliveryAddressDetails");

			_deliveryStreet = deliveryStreet.Trim();
			_deliveryAddressDetails = deliveryAddressDetails.Trim();
			_recipient = recipient;
		}

		public ApplicationUser Recipient
		{
			get
			{
				return _recipient;
			}
		}
		public string DeliveryStreet
		{
			get
			{
				return _deliveryStreet;
			}
		}
		public string DeliveryAddressDetails
		{
			get
			{
				return _deliveryAddressDetails;
			}
		}
		public ICollection<OrderedProduct> OrderedProducts
		{
			get
			{
				return _orderedProducts;
			}
		}

		private readonly ApplicationUser _recipient;
		private readonly string _deliveryStreet;
		private readonly string _deliveryAddressDetails;
		private readonly ICollection<OrderedProduct> _orderedProducts = new OrderedProductsCollection();
	}
}