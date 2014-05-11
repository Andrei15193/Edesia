using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.ViewModels.Product
{
	public class ProductsViewModel
	{
		public ProductsViewModel(IEnumerable<Models.Product> products, bool isUserAdministrator)
		{
			if (products == null)
				throw new ArgumentNullException("products");

			_isUserAdministrator = isUserAdministrator;
			_products = products;
		}

		public bool IsUserAdministrator
		{
			get
			{
				return _isUserAdministrator;
			}
		}
		public IEnumerable<ProductViewModel> ProductViewModels
		{
			get
			{
				return _products.Select(product => new ProductViewModel(product, _isUserAdministrator));
			}
		}

		private readonly bool _isUserAdministrator;
		private readonly IEnumerable<Models.Product> _products;
	}
}