using System;
using System.Collections.Generic;
using System.Linq;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Mock
{
	public class MockProductRepository
		: IProductRepository
	{
		#region IProductRepository Members
		public IEnumerable<Product> GetProducts()
		{
			return _products.Where(product => !product.DateRemoved.HasValue);
		}

		public Product GetProduct(string name)
		{
			return _products.FirstOrDefault(product => product.Name == name);
		}
		public void AddProduct(Product product)
		{
			if (product == null)
				throw new ArgumentNullException("product");

			if (_products.Any(exitingProduct => exitingProduct.Name == product.Name
												&& (!exitingProduct.DateRemoved.HasValue || exitingProduct.DateRemoved.Value >= product.DateAdded)
												&& (!product.DateRemoved.HasValue || product.DateRemoved.Value >= exitingProduct.DateAdded)))
				throw new AggregateException(new UniqueProductException(product.Name));

			_products.Add(product);
		}
		public void RemoveProduct(string name)
		{
			Product productToRemove = _products.FirstOrDefault(product => product.Name == name);

			if (productToRemove != null)
				productToRemove.DateRemoved = DateTime.Now;
		}
		#endregion

		private readonly IList<Product> _products = new List<Product>
		{
			new Product("Lapte Napolact 1L", 10, DateTime.Now.AddDays(-10)),
			new Product("Iaurt Napolact 1L", 10, DateTime.Now.AddDays(-10)),
			new Product("Smantana Napolact 1L", 10, DateTime.Now.AddDays(-10)),
			new Product("Ciocolata Milka 1L", 10, DateTime.Now.AddDays(-10)),
			new Product("Ciocolata Poiana 1L", 10, DateTime.Now.AddDays(-10)),
			new Product("Ciocolata Laura 1L", 10, DateTime.Now.AddDays(-10)),
		};
	}
}