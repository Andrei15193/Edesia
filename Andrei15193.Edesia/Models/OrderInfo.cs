using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
namespace Andrei15193.Edesia.Models
{
	public class OrderInfo
	{
		public OrderInfo(DateTime datePlaced, string recipientEMailAddress, IEnumerable<OrderedProduct> orderedProducts, string deliveryAddress, string deliveryAddressLine2 = null, OrderState state = OrderState.Pending)
		{
			if (recipientEMailAddress == null)
				throw new ArgumentNullException("recipientEMailAddress");
			try
			{
				recipientEMailAddress = new MailAddress(recipientEMailAddress).Address;
			}
			catch (FormatException formatException)
			{
				throw new ArgumentException("Must be an e-mail address!", "recipientEMailAddress", formatException);
			}

			if (orderedProducts == null)
				throw new ArgumentNullException("orderedProducts");

			if (deliveryAddress == null)
				throw new ArgumentNullException("deliveryAddress");
			if (string.IsNullOrWhiteSpace(deliveryAddress))
				throw new ArgumentException("Cannot be empty or whitespace!", "deliveryAddress");

			if (string.IsNullOrWhiteSpace(deliveryAddressLine2))
				_deliveryAddressLine2 = null;
			else
				_deliveryAddressLine2 = deliveryAddressLine2.Trim();

			_datePlaced = datePlaced;
			_orderedProducts = orderedProducts.ToList();
			_deliveryAddress = deliveryAddress.Trim();
			_state = state;
		}

		public DateTime DatePlaced
		{
			get
			{
				return _datePlaced;
			}
		}
		public string RecipientEMailAddress
		{
			get
			{
				return _recipientEMailAddress;
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
			get
			{
				return _state;
			}
		}
		public IReadOnlyCollection<OrderedProduct> OrderedProducts
		{
			get
			{
				return _orderedProducts;
			}
		}

		private readonly DateTime _datePlaced;
		private readonly string _recipientEMailAddress;
		private readonly string _deliveryAddressLine2;
		private readonly string _deliveryAddress;
		private readonly OrderState _state;
		private readonly IReadOnlyCollection<OrderedProduct> _orderedProducts;
	}
}