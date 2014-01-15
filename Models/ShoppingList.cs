using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class ShoppingList
	{
		public ShoppingList(string name, User user)
		{
			Name = name;
			User = user;
		}

		public string Name
		{
			get;
			private set;
		}
		public User User
		{
			get;
			private set;
		}

		public void Add(Offer offer, uint quantity)
		{
			_offers.Add(offer, quantity);
		}
		public void Update(Offer offer, uint quantity)
		{
			_offers[offer] = quantity;
		}
		public void Remove(Offer offer)
		{
			_offers.Remove(offer);
		}

		private readonly IDictionary<Offer, uint> _offers = new Dictionary<Offer, uint>();
	}
}