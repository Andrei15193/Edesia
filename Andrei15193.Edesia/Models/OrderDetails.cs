using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models.Collections;
namespace Andrei15193.Edesia.Models
{
	public sealed class OrderDetails
	{
		public OrderDetails(ApplicationUser recipient, string deliveryAddress, string deliveryAddressLine2 = null)
		{
			if (recipient == null)
				throw new ArgumentNullException("recipient");

			if (deliveryAddress == null)
				throw new ArgumentNullException("deliveryAddress");
			if (string.IsNullOrWhiteSpace(deliveryAddress))
				throw new ArgumentException("Cannot be empty or whitespace!", "deliveryAddress");

			if (string.IsNullOrWhiteSpace(deliveryAddressLine2))
				_deliveryAddressLine2 = null;
			else
				_deliveryAddressLine2 = deliveryAddressLine2.Trim();

			_deliveryAddress = deliveryAddress.Trim();
			_recipient = recipient;
		}

		public ApplicationUser Recipient
		{
			get
			{
				return _recipient;
			}
		}
		public string DeliveryAddress
		{
			get
			{
				return _deliveryAddress;
			}
		}
		public string DeliveryAddressLine2
		{
			get
			{
				return _deliveryAddressLine2;
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
		private readonly string _deliveryAddressLine2;
		private readonly string _deliveryAddress;
		private readonly ICollection<OrderedProduct> _orderedProducts = new OrderedProductsCollection();
	}
}