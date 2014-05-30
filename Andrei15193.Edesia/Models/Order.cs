using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class Order
	{
		public Order(int orderNumber, DateTime datePlaced, ApplicationUser recipient, string deliveryStreet, string deliveryAddressDetails, OrderState orderState = OrderState.Pending)
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

			_orderNumber = orderNumber;
			_datePlaced = datePlaced;
			_recipient = recipient;
			_deliveryStreet = deliveryStreet.Trim();
			_deliveryAddressDetails = deliveryAddressDetails.Trim();
			State = orderState;
		}

		public int OrderNumber
		{
			get
			{
				return _orderNumber;
			}
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
		public OrderState State
		{
			get;
			set;
		}
		public ISet<OrderedProduct> OrderedProducts
		{
			get
			{
				return _orderedProducts;
			}
		}

		public double TotalCapacity
		{
			get
			{
				return _orderedProducts.Sum(orderedProduct => orderedProduct.Product.Capacity * orderedProduct.Quantity);
			}
		}
		public double TotalCost
		{
			get
			{
				return _orderedProducts.Sum(orderedProduct => orderedProduct.Product.Price * orderedProduct.Quantity);
			}
		}

		private readonly int _orderNumber;
		private readonly DateTime _datePlaced;
		private readonly ApplicationUser _recipient;
		private readonly string _deliveryStreet;
		private readonly string _deliveryAddressDetails;
		private readonly ISet<OrderedProduct> _orderedProducts = new HashSet<OrderedProduct>();
	}
}