using System;
namespace Andrei15193.Edesia.Models
{
	public struct ShoppingCartEntry
	{
		public ShoppingCartEntry(Product product, int quantity)
		{
			if (product == null)
				throw new ArgumentNullException("product");
			if (quantity <= 0)
				throw new ArgumentException("Must be strictly positive.", "quantity");

			_product = product;
			_quantity = quantity;
		}

		public Product Product
		{
			get
			{
				return _product;
			}
		}
		public int Quantity
		{
			get
			{
				return _quantity;
			}
		}

		private readonly Product _product;
		private readonly int _quantity;
	}
}