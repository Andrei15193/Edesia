using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	[Obsolete]
	public interface IProductRepository
		: IProductProvider
	{
		Product GetProduct(string productName);
		IEnumerable<Product> GetProducts();
		
		void Add(Product product);
		void Remove(string productName);
	}
}