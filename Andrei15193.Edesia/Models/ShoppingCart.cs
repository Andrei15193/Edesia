using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models
{
	public class ShoppingCart
		: IReadOnlyCollection<OrderedProduct>
	{
		public ShoppingCart(ApplicationUser owner, IEnumerable<OrderedProduct> orderedProducts)
		{
			if (owner == null)
				throw new ArgumentNullException("owner");
			if (orderedProducts == null)
				throw new ArgumentNullException("orderedProducts");

			_owner = owner;
			_orderedProducts = orderedProducts;
		}

		#region IReadOnlyCollection<OrderedProduct> Members
		public int Count
		{
			get
			{
				return _orderedProducts.Sum(product => product.Quantity);
			}
		}
		#endregion
		#region IEnumerable<OrderedProduct> Members
		public IEnumerator<OrderedProduct> GetEnumerator()
		{
			return _orderedProducts.GetEnumerator();
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
				return _orderedProducts.Sum(orderedProduct => orderedProduct.Product.Price * orderedProduct.Quantity);
			}
		}

		private readonly ApplicationUser _owner;
		private readonly IEnumerable<OrderedProduct> _orderedProducts;
	}
}