using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Controllers
{
	[ConfirmAccess]
	public class ShoppingCartController
		: ApplicationController
	{
		[HttpGet]
		public ActionResult Default()
		{
			return View(_applicationUserRepository.GetShoppingCart(User, _productProvider));
		}

		[HttpGet]
		[AllowAnonymous]
		public JsonResult DefaultJson()
		{
			if (User == null)
				return Json(new
							{
								Count = 0,
								TotalPrice = 0d,
								Items = new object[0]
							},
							JsonRequestBehavior.AllowGet);
			else
			{
				ShoppingCart shoppingCart = _applicationUserRepository.GetShoppingCart(User, _productProvider);

				return Json(new
							{
								Count = shoppingCart.Count,
								TotalPrice = shoppingCart.TotoalPrice,
								Items = shoppingCart.Select(orderedProduct => new
									{
										Product = new
											{
												Name = orderedProduct.Product.Name,
												Price = orderedProduct.Product.Price,
												ImageUri = orderedProduct.Product.ImageLocation.AbsoluteUri
											},
										Quantity = orderedProduct.Quantity
									}).ToArray()
							},
							JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public ActionResult Add(string product, int quantity = 1)
		{
			if (product != null)
				_applicationUserRepository.AddToCart(User, new OrderedProduct(_productProvider.GetProduct(product), quantity));

			return RedirectToAction("Default", "ShoppingCart");
		}
		[HttpGet]
		public ActionResult Update(string product, int quantity)
		{
			if (product != null)
				if (quantity > 0)
					_applicationUserRepository.UpdateCart(User, new OrderedProduct(_productProvider.GetProduct(product), quantity));
				else
					_applicationUserRepository.RemoveFromCart(User, _productProvider.GetProduct(product));

			return RedirectToAction("Default", "ShoppingCart");
		}
		[HttpGet]
		public ActionResult Remove(string product)
		{
			if (product != null)
				_applicationUserRepository.RemoveFromCart(User, _productProvider.GetProduct(product));

			return RedirectToAction("Default", "ShoppingCart");
		}

		private readonly IProductProvider _productProvider = (IProductProvider)MvcApplication.DependencyContainer["productRepository"];
		private readonly IApplicationUserRepository _applicationUserRepository = (IApplicationUserRepository)MvcApplication.DependencyContainer["applicationUserRepository"];
	}
}