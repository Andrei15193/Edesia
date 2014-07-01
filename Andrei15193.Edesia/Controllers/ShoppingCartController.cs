using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Controllers
{
	public class ShoppingCartController
		: ApplicationController
	{
		[HttpGet, Authorize]
		public ActionResult Default()
		{
			return View(_userRepository.GetShoppingCart(User));
		}

		[HttpGet]
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
				ShoppingCart shoppingCart = _userRepository.GetShoppingCart(User);

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

		[HttpGet, Authorize]
		public ActionResult Add(string product, int quantity = 1)
		{
			if (User == null)
				return RedirectToAction("Login", "User", new
					{
						returnUrl = Request.Url.AbsoluteUri
					});

			if (product != null)
				_userRepository.AddToShoppingCart(User, new ShoppingCartEntry(_productRepository.GetProduct(Server.UrlDecode(product)), quantity));

			return RedirectToAction("Default", "ShoppingCart");
		}
		[HttpGet, Authorize]
		public ActionResult Update(string product, int quantity = 1)
		{
			product = Server.UrlDecode(product);
			if (product != null)
				if (quantity > 0)
					_userRepository.UpdateShoppingCart(User, new ShoppingCartEntry(_productRepository.GetProduct(Server.UrlDecode(product)), quantity));
				else
					_userRepository.RemoveFromShoppingCart(User, _productRepository.GetProduct(Server.UrlDecode(product)));

			return RedirectToAction("Default", "ShoppingCart");
		}
		[HttpGet, Authorize]
		public ActionResult Remove(string product)
		{
			if (product != null)
				_userRepository.RemoveFromShoppingCart(User, _productRepository.GetProduct(Server.UrlDecode(product)));

			return RedirectToAction("Default", "ShoppingCart");
		}

		private readonly IUserRepository _userRepository = (IUserRepository)MvcApplication.DependencyContainer["userRepository"];
		private readonly IProductRepository _productRepository = (IProductRepository)MvcApplication.DependencyContainer["productRepo"];
	}
}