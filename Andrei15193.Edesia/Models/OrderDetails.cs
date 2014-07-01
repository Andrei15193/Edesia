using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models.Collections;
namespace Andrei15193.Edesia.Models
{
	public struct OrderDetails
	{
		public OrderDetails(ApplicationUser recipient, DeliveryAddress deliveryAddress, DateTime datePlaced)
		{
			if (recipient == null)
				throw new ArgumentNullException("recipient");

			_deliveryAddress = deliveryAddress;
			_recipient = recipient;
			_datePlaced = datePlaced;
			_orderedProducts = new OrderedProductsCollection();
		}

		public DateTime DatePlaced
		{
			get
			{
				return _datePlaced;
			}
		}
		public ApplicationUser Recipient
		{
			get
			{
				return _recipient;
			}
		}
		public DeliveryAddress DeliveryAddress
		{
			get
			{
				return _deliveryAddress;
			}
		}
		public ICollection<OrderedProduct> OrderedProducts
		{
			get
			{
				return _orderedProducts;
			}
		}

		private readonly DateTime _datePlaced;
		private readonly ApplicationUser _recipient;
		private readonly DeliveryAddress _deliveryAddress;
		private readonly ICollection<OrderedProduct> _orderedProducts;
	}
}