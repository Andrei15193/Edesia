using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class ShoppingCart
		: IReadOnlyCollection<ShoppingCartEntry>
	{
		public ShoppingCart(ApplicationUser owner, IEnumerable<ShoppingCartEntry> shoppingCartEntries)
		{
			if (owner == null)
				throw new ArgumentNullException("owner");
			if (shoppingCartEntries == null)
				throw new ArgumentNullException("shoppingCartEntries");

			_owner = owner;
			_shoppingCartEntries = shoppingCartEntries;
		}

		#region IReadOnlyCollection<ShoppingCartEntry> Members
		public int Count
		{
			get
			{
				return _shoppingCartEntries.Sum(product => product.Quantity);
			}
		}
		#endregion
		#region IEnumerable<ShoppingCartEntry> Members
		public IEnumerator<ShoppingCartEntry> GetEnumerator()
		{
			return _shoppingCartEntries.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		public ApplicationUser Owner
		{
			get
			{
				return _owner;
			}
		}
		public double TotoalPrice
		{
			get
			{
				return _shoppingCartEntries.Sum(orderedProduct => orderedProduct.Product.Price * orderedProduct.Quantity);
			}
		}

		private readonly ApplicationUser _owner;
		private readonly IEnumerable<ShoppingCartEntry> _shoppingCartEntries;
	}
}