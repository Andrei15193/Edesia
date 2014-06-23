using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class Order
	{
		public Order(int number, DateTime datePlaced, ApplicationUser recipient, DeliveryAddress deliveryAddress, OrderState orderState = OrderState.Pending)
		{
			if (recipient == null)
				throw new ArgumentNullException("recipient");

			_number = number;
			_datePlaced = datePlaced;
			_recipient = recipient;
			_deliveryAddress = deliveryAddress;
			State = orderState;
		}

		public int Number
		{
			get
			{
				return _number;
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
		public DeliveryAddress DeliveryAddress
		{
			get
			{
				return _deliveryAddress;
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
		public static IEqualityComparer<Order> IdentityComparer
		{
			get
			{
				return _identityComparer;
			}
		}

		private readonly int _number;
		private readonly DateTime _datePlaced;
		private readonly ApplicationUser _recipient;
		private readonly DeliveryAddress _deliveryAddress;
		private readonly ISet<OrderedProduct> _orderedProducts = new SortedSet<OrderedProduct>(_orderedProductComparer);
		private readonly static IComparer<OrderedProduct> _orderedProductComparer = new OrderedProductComparer();
		private readonly static IEqualityComparer<Order> _identityComparer = new OrderIdentityComparer();

		private sealed class OrderIdentityComparer
			: IEqualityComparer<Order>
		{
			#region IEqualityComparer<Order> Members
			public bool Equals(Order one, Order another)
			{
				if (one == null)
					return (another == null);
				else
					return (another != null
							&& one._number == another._number);
			}
			public int GetHashCode(Order value)
			{
				if (value == null)
					throw new ArgumentNullException("value");

				return value._number.GetHashCode();
			}
			#endregion
		}

		private sealed class OrderedProductComparer
			: IComparer<OrderedProduct>
		{
			#region IComparer<OrderedProduct> Members
			public int Compare(OrderedProduct first, OrderedProduct second)
			{
				return string.Compare(first.Product.Name, second.Product.Name, StringComparison.OrdinalIgnoreCase);
			}
			#endregion
		}
	}
}