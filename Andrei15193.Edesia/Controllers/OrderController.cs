using System;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.Order;
namespace Andrei15193.Edesia.Controllers
{
	public class OrderController
		: ApplicationController
	{
		[ChildActionOnly]
		public ActionResult Default()
		{
			return View(_deliveryRepository.GetOrders(OrderState.Pending));
		}

		[HttpGet, Authorize]
		public ActionResult Checkout()
		{
			ShoppingCart shoppingCart = _userRepository.GetShoppingCart(User);
			if (shoppingCart.Count == 0)
				return RedirectToAction("Default", "Product");

			return View(new CheckoutViewModel
						{
							ShoppingCart = shoppingCart,
							Streets = _deliveryRepository.GetStreets()
						});
		}
		[HttpPost, Authorize]
		public ActionResult Checkout(CheckoutViewModel checkoutViewModel)
		{
			ShoppingCart shoppingCart = _userRepository.GetShoppingCart(User);
			if (shoppingCart.Count == 0)
				return RedirectToAction("Default", "Product");

			if (!ModelState.IsValid)
			{
				checkoutViewModel.ShoppingCart = shoppingCart;
				checkoutViewModel.Streets = _deliveryRepository.GetStreets();

				return View(checkoutViewModel);
			}

			OrderDetails orderDetails = new OrderDetails(User,
														 new DeliveryAddress(checkoutViewModel.SelectedStreet, checkoutViewModel.AddressDetails),
														 DateTime.Now);
			foreach (ShoppingCartEntry shoppingCartEntry in shoppingCart)
				orderDetails.OrderedProducts.Add(new OrderedProduct(shoppingCartEntry.Product, shoppingCartEntry.Quantity));

			_deliveryRepository.Add(orderDetails);
			_userRepository.ClearShoppingCart(User);

			return View("_Notice", new Notice(OrderControllerStrings.CheckoutViewTitle, null, OrderControllerStrings.ThanksNoticeParagraph1, OrderControllerStrings.ThanksNoticeParagraph2, OrderControllerStrings.ThanksNoticeAuthor));
		}

		[HttpGet, Authorize]
		public ActionResult Registry()
		{
			return View(_deliveryRepository.GetOrders(User, OrderState.EnRoute, OrderState.Pending, OrderState.Scheduled, OrderState.Delivered));
		}

		[HttpGet, Authorize, Role(typeof(Administrator))]
		public JsonResult PendingCountJson()
		{
			return Json(new
				{
					Count = _deliveryRepository.GetOrders(OrderState.Pending).Count()
				},
				JsonRequestBehavior.AllowGet);
		}

		private readonly IUserRepository _userRepository = (IUserRepository)MvcApplication.DependencyContainer["userRepository"];
		private readonly IDeliveryRepository _deliveryRepository = (IDeliveryRepository)MvcApplication.DependencyContainer["deliveryRepo"];
	}
}