using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class Stats
	{
		public Stats(IEnumerable<Order> orders)
		{
			_orders = orders.ToList();
		}

		public int NumberOfOffersPurchased
		{
			get
			{
				return _orders.Count;
			}
		}

		private readonly IReadOnlyCollection<Order> _orders;
	}
}