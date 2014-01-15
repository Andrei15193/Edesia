using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class Order
	{
		public Order(User user, ShoppingCart shoppingCart)
		{
			User = user;
			TimePurchased = DateTime.Now;
			foreach (KeyValuePair<Offer, uint> offer in shoppingCart)
				_offersBought.Add(offer.Key, offer.Value);
		}

		public OrderState State
		{
			get;
			set;
		}
		public DateTime TimePurchased
		{
			get;
			private set;
		}
		public User User
		{
			get;
			private set;
		}
		public IReadOnlyDictionary<Offer, uint> OffersBought
		{
			get
			{
				return _offersBought;
			}
		}

		private readonly Dictionary<Offer, uint> _offersBought = new Dictionary<Offer, uint>();
	}
}