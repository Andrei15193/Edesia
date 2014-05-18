using System.Collections.Generic;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess
{
	public interface IProductRepository
		: IProductProvider
	{
		IEnumerable<Product> GetProducts();

		void AddProduct(Product product);
		void RemoveProduct(string name);
	}
}