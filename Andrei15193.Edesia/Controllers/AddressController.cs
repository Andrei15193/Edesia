using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Collections;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.Address;
namespace Andrei15193.Edesia.Controllers
{
	public class AddressController
		: ApplicationController
	{
		[ChildActionOnly]
		public ActionResult Unmapped()
		{
			IEnumerable<string> unusedAddress = new SortedSet<string>(_deliveryRepository.GetUnmappedAddresses().Except(_orderRepository.GetUsedAddresses()), StringComparer.Ordinal);

			return View(_deliveryRepository.GetUnmappedAddresses()
										   .Select(address => KeyValuePair.Create(address, unusedAddress.Contains(address))));
		}

		[HttpGet]
		public ActionResult Add()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Add(AddAddressViewModel addAddressViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.AddAddress(addAddressViewModel.Address);
					return Redirect(Url.Action("Default", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueAddressException uniqueAddressException = aggregatedException as UniqueAddressException;

						if (uniqueAddressException != null)
							ModelState.AddModelError("Address", string.Format(ErrorStrings.AddressTextBox_InvalidDuplicateValue_Format, uniqueAddressException.ConflictingValue));
					}

					return View(addAddressViewModel);
				}
			}
			else
				return View(addAddressViewModel);
		}

		[HttpGet]
		public ActionResult Remove(string address)
		{
			if (address != null)
				_deliveryRepository.RemoveAddress(address);

			return RedirectToAction("Default", "Delivery");
		}

		private readonly IOrderProvider _orderRepository = (IOrderProvider)MvcApplication.DependencyContainer["orderRepository"];
		private readonly IDeliveryZoneRepository _deliveryRepository = (IDeliveryZoneRepository)MvcApplication.DependencyContainer["deliveryRepository"];
	}
}