using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class Order
	{
		public Order(int orderNumber, DateTime datePlaced, ApplicationUser recipient, string deliveryAddress, string deliveryAddressLine2 = null, OrderState orderState = OrderState.Pending)
		{
			if (recipient == null)
				throw new ArgumentNullException("recipient");

			if (deliveryAddress == null)
				throw new ArgumentNullException("deliveryAddress");
			if (string.IsNullOrWhiteSpace(deliveryAddress))
				throw new ArgumentException("Cannot be empty or whitespace!", "deliveryAddress");

			_orderNumber = orderNumber;
			_datePlaced = datePlaced;
			_recipient = recipient;
			_deliveryAddress = deliveryAddress.Trim();
			State = orderState;

			if (string.IsNullOrWhiteSpace(deliveryAddressLine2))
				_deliveryAddressLine2 = null;
			else
				_deliveryAddressLine2 = deliveryAddressLine2.Trim();
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

		public int TotalCapacity
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
		private readonly string _deliveryAddress;
		private readonly string _deliveryAddressLine2;
		private readonly ISet<OrderedProduct> _orderedProducts = new HashSet<OrderedProduct>();
	}
}