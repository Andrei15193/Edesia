using System.Collections;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public class ShoppingCart
		: IEnumerable<KeyValuePair<Offer, uint>>
	{
		#region IEnumerable<KeyValuePair<Offer,uint>> Members
		public IEnumerator<KeyValuePair<Offer, uint>> GetEnumerator()
		{
			return _offers.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _offers.GetEnumerator();
		}
		#endregion
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