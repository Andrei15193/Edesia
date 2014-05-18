using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Models.Collections
{
	public sealed class OrderedProductsCollection
		: ICollection<OrderedProduct>
	{
		#region ICollection<OrderedProduct> Members
		void ICollection<OrderedProduct>.Add(OrderedProduct orderedProduct)
		{
			int existingQuantity;

			if (_productsWithQuantity.TryGetValue(orderedProduct.Product, out existingQuantity))
				_productsWithQuantity[orderedProduct.Product] = orderedProduct.Quantity + existingQuantity;
			else
				_productsWithQuantity.Add(orderedProduct.Product, orderedProduct.Quantity);
		}
		public void Clear()
		{
			_productsWithQuantity.Clear();
		}
		public bool Contains(OrderedProduct orderedProduct)
		{
			return _productsWithQuantity.ContainsKey(orderedProduct.Product);
		}
		public void CopyTo(OrderedProduct[] array, int arrayIndex)
		{
			_productsWithQuantity.Select(productWithQuantity => new OrderedProduct(productWithQuantity.Key, productWithQuantity.Value))
								 .ToList()
								 .CopyTo(array, arrayIndex);
		}
		public int Count
		{
			get
			{
				return _productsWithQuantity.Count;
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		public bool Remove(OrderedProduct orderedProduct)
		{
			return _productsWithQuantity.Remove(orderedProduct.Product);
		}
		#endregion
		#region IEnumerable<OrderedProduct> Members
		public IEnumerator<OrderedProduct> GetEnumerator()
		{
			return _productsWithQuantity.Select(productWithQuantity => new OrderedProduct(productWithQuantity.Key, productWithQuantity.Value))
										.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

		private readonly IDictionary<Product, int> _productsWithQuantity = new Dictionary<Product, int>();
	}
}