using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.Delivery;
namespace Andrei15193.Edesia.Controllers
{
	[ConfirmAccess(typeof(Administrator))]
	public class DeliveryController
		: ApplicationController
	{
		[HttpGet]
		public ActionResult Planning()
		{
			return View(new DeliveryPlanningViewModel(new DeliveryZonesViewModel(_deliveryRepository.GetUnmappedAddresses(), _deliveryRepository.GetDeliveryZones(), _GetUnuesdAddresses())));
		}
		[HttpGet]
		public ActionResult ManageDeliveryZones()
		{
			return View(new DeliveryZonesViewModel(_deliveryRepository.GetUnmappedAddresses(), _deliveryRepository.GetDeliveryZones(), _GetUnuesdAddresses()));
		}

		[HttpGet]
		public ActionResult AddAddress()
		{
			return View();
		}
		[HttpPost]
		public ActionResult AddAddress(AddAddressViewModel addAddressViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.AddAddress(addAddressViewModel.Address);
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
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
		public ActionResult RemoveAddress()
		{
			RemoveAddressViewModel removeAddressesViewModel = new RemoveAddressViewModel();

			foreach (string unusedAddress in _GetUnuesdAddresses())
				removeAddressesViewModel.UnusedAddresses.Add(unusedAddress);

			return View(removeAddressesViewModel);
		}
		[HttpPost]
		public ActionResult RemoveAddress(RemoveAddressViewModel removeAddressViewModel)
		{
			if (ModelState.IsValid)
			{
				_deliveryRepository.RemoveAddress(removeAddressViewModel.AddressToRemove);
				return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
			}
			else
				return View(removeAddressViewModel);
		}

		[HttpGet]
		public ActionResult AddDeliveryZone()
		{
			return View(new DeliveryZoneViewModel(_deliveryRepository.GetUnmappedAddresses().Select(unmappedAddress => new KeyValuePair<string, bool>(unmappedAddress, false)))
				{
					SubmitButtonText = AddDeliveryZoneViewStrings.SubmitButton_DisplayName
				});
		}
		[HttpPost]
		public ActionResult AddDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.AddDeliveryZone(_GetDeliveryZone(deliveryZoneViewModel));
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueDeliveryZoneNameException uniqueDeliveryZoneNameException = aggregatedException as UniqueDeliveryZoneNameException;

						if (uniqueDeliveryZoneNameException != null)
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueDeliveryZoneNameException.ConflictingValue));
					}
				}
			}

			deliveryZoneViewModel.AvailableAddresses.Clear();
			foreach (string unmappedAddress in _deliveryRepository.GetUnmappedAddresses())
				deliveryZoneViewModel.AvailableAddresses.Add(new KeyValuePair<string, bool>(unmappedAddress, Request.Form.AllKeys.Contains("checkbox " + unmappedAddress)));

			return View(deliveryZoneViewModel);
		}

		[HttpGet]
		public ActionResult GetDeliveryZoneName()
		{
			return View(_deliveryRepository.GetDeliveryZones());
		}
		[HttpGet]
		public ActionResult EditDeliveryZone(string deliveryZoneName)
		{
			if (deliveryZoneName != null)
			{
				DeliveryZone deliveryZoneFound = _deliveryRepository.GetDeliveryZones().FirstOrDefault(deliveryZone => string.Equals(deliveryZoneName, deliveryZone.Name, StringComparison.OrdinalIgnoreCase));
				if (deliveryZoneFound != null)
				{
					DeliveryZoneViewModel deliveryZoneViewModel = new DeliveryZoneViewModel(deliveryZoneFound.Addresses.Select(address => new KeyValuePair<string, bool>(address, true))
																											 .Concat(_deliveryRepository.GetUnmappedAddresses().Select(address => new KeyValuePair<string, bool>(address, false))))
					{
						DeliveryZoneName = deliveryZoneFound.Name,
						DeliveryZoneColour = deliveryZoneFound.Colour.ToString(),
						DeliveryZoneOldName = deliveryZoneName,
						SubmitButtonText = EditDeliveryZoneViewStrings.SubmitButton_DisplayName
					};

					return View(deliveryZoneViewModel);
				}
				else
					ModelState.AddModelError("deliveryZoneName", ErrorStrings.DeliveryZoneNameComboBox_InvalidValue);
			}

			return View("GetDeliveryZoneName", _deliveryRepository.GetDeliveryZones());
		}
		[HttpPost]
		public ActionResult EditDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.UpdateDeliveryZone(_GetDeliveryZone(deliveryZoneViewModel), deliveryZoneViewModel.DeliveryZoneOldName);
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueDeliveryZoneNameException uniqueDeliveryZoneNameException = aggregatedException as UniqueDeliveryZoneNameException;

						if (uniqueDeliveryZoneNameException != null)
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueDeliveryZoneNameException.ConflictingValue));
					}
				}
			}

			DeliveryZone deliveryZoneFound = _deliveryRepository.GetDeliveryZones().FirstOrDefault(deliveryZone => string.Equals(deliveryZoneViewModel.DeliveryZoneOldName, deliveryZone.Name, StringComparison.OrdinalIgnoreCase));
			if (deliveryZoneFound != null)
			{
				deliveryZoneViewModel.AvailableAddresses.Clear();
				foreach (KeyValuePair<string, bool> address in deliveryZoneFound.Addresses.Select(address => new KeyValuePair<string, bool>(address, true))
																						  .Concat(Request.Form.Keys.Cast<string>()
																												   .Where(inputName => inputName.StartsWith("checkbox "))
																												   .Select(inputName => new KeyValuePair<string, bool>(inputName.Substring(9), true)))
																						  .Concat(_deliveryRepository.GetUnmappedAddresses().Select(address => new KeyValuePair<string, bool>(address, false))))
					deliveryZoneViewModel.AvailableAddresses.Add(address);
			}
			else
				return EditDeliveryZone(deliveryZoneViewModel.DeliveryZoneName);

			deliveryZoneViewModel.SubmitButtonText = EditDeliveryZoneViewStrings.SubmitButton_DisplayName;
			return View(deliveryZoneViewModel);
		}
		[HttpGet]
		public ActionResult RemoveDeliveryZone()
		{
			return View(_deliveryRepository.GetDeliveryZones());
		}
		[HttpPost]
		public ActionResult RemoveDeliveryZone(string deliveryZoneName)
		{
			if (deliveryZoneName != null)
				try
				{
					_deliveryRepository.RemoveDeliveryZone(deliveryZoneName);
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueDeliveryZoneNameException uniqueDeliveryZoneNameException = aggregatedException as UniqueDeliveryZoneNameException;

						if (uniqueDeliveryZoneNameException != null)
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueDeliveryZoneNameException.ConflictingValue));
					}
				}

			return RemoveDeliveryZone();
		}

		private IEnumerable<string> _GetUnuesdAddresses()
		{
			return _deliveryRepository.GetUnmappedAddresses()
									  .Except(_applicationUserRepository.GetAddresses().Select(detailedAddress => detailedAddress.Address), StringComparer.OrdinalIgnoreCase)
									  .OrderBy(address => address);
		}
		private DeliveryZone _GetDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			return new DeliveryZone(deliveryZoneViewModel.DeliveryZoneName,
									Colour.Parse(deliveryZoneViewModel.DeliveryZoneColour),
									Request.Form.Keys.Cast<string>()
													 .Where(inputName => inputName.StartsWith("checkbox "))
													 .Select(inputName => inputName.Substring(9)));

		}

		private readonly IApplicationUserRepository _applicationUserRepository = (IApplicationUserRepository)MvcApplication.DependencyContainer["applicationUserRepository"];
		private readonly IDeliveryRepository _deliveryRepository = (IDeliveryRepository)MvcApplication.DependencyContainer["deliveryRepository"];
	}
}