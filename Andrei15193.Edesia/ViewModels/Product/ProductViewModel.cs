using System;
namespace Andrei15193.Edesia.ViewModels.Product
{
	public class ProductViewModel
	{
		public ProductViewModel(Models.Product product, bool isUserAdministrator)
		{
			if (product == null)
				throw new ArgumentNullException("product");

			_product = product;
			_isUserAdministrator = isUserAdministrator;
		}

		public bool IsUserAdministrator
		{
			get
			{
				return _isUserAdministrator;
			}
		}
		public Models.Product Product
		{
			get
			{
				return _product;
			}
		}

		private readonly bool _isUserAdministrator;
		private readonly Models.Product _product;
	}
}