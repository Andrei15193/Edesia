using System.Web.Mvc;
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
			return View(_orderRepository.GetOrders(_applicationUserRepository, _productProvider, OrderState.Pending));
		}

		[HttpGet, Authorize]
		public ActionResult Checkout()
		{
			ShoppingCart shoppingCart = _applicationUserRepository.GetShoppingCart(User, _productProvider);
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
			ShoppingCart shoppingCart = _applicationUserRepository.GetShoppingCart(User, _productProvider);
			if (shoppingCart.Count == 0)
				return RedirectToAction("Default", "Product");

			if (!ModelState.IsValid)
			{
				checkoutViewModel.ShoppingCart = shoppingCart;
				checkoutViewModel.Streets = _deliveryRepository.GetStreets();

				return View(checkoutViewModel);
			}

			OrderDetails orderDetails = new OrderDetails(User, checkoutViewModel.SelectedStreet, checkoutViewModel.AddressDetails);
			foreach (OrderedProduct orderedProduct in shoppingCart)
				orderDetails.OrderedProducts.Add(orderedProduct);

			_orderRepository.PlaceOrder(orderDetails);
			_applicationUserRepository.ClearShoppingCart(User);

			return View("_Notice", new Notice(OrderControllerStrings.CheckoutViewTitle, null, OrderControllerStrings.ThanksNoticeParagraph1, OrderControllerStrings.ThanksNoticeParagraph2, OrderControllerStrings.ThanksNoticeAuthor));
		}

		private readonly IOrderRepository _orderRepository = (IOrderRepository)MvcApplication.DependencyContainer["orderRepository"];
		private readonly IProductProvider _productProvider = (IProductProvider)MvcApplication.DependencyContainer["productRepository"];
		private readonly IDeliveryZoneProvider _deliveryRepository = (IDeliveryZoneProvider)MvcApplication.DependencyContainer["deliveryRepository"];
		private readonly IApplicationUserRepository _applicationUserRepository = (IApplicationUserRepository)MvcApplication.DependencyContainer["applicationUserRepository"];
	}
}