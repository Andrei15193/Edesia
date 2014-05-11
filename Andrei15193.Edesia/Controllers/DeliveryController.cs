using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.Delivery;
using Andrei15193.Edesia.Xml.Validation;
namespace Andrei15193.Edesia.Controllers
{
	[ConfirmAccess(typeof(Administrator))]
	public class DeliveryController
		: ApplicationController
	{
		[HttpGet]
		public ActionResult Planning()
		{
			return View(new DeliveryPlanningViewModel(new DeliveryZonesViewModel(_deliveryRepository.GetUnmappedStreets(), _deliveryRepository.GetDeliveryZones(), _GetUnuesdStreets())));
		}
		[HttpGet]
		public ActionResult ManageDeliveryZones()
		{
			return View(new DeliveryZonesViewModel(_deliveryRepository.GetUnmappedStreets(), _deliveryRepository.GetDeliveryZones(), _GetUnuesdStreets()));
		}

		[HttpGet]
		public ActionResult AddStreet()
		{
			return View();
		}
		[HttpPost]
		public ActionResult AddStreet(AddStreetViewModel addStreetViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.AddStreet(addStreetViewModel.StreetName);
					return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueConstraintException uniqueConstraintException = (aggregatedException as UniqueConstraintException);

						if (uniqueConstraintException != null && string.Equals(uniqueConstraintException.ConstraintName, "http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd:UniqueStreetNames", StringComparison.Ordinal))
							ModelState.AddModelError("StreetName", string.Format(ErrorStrings.StreetTextBox_InvalidDuplicateValue_Format, uniqueConstraintException.ConflictingValue));
					}

					return View(addStreetViewModel);
				}
			}
			else
				return View(addStreetViewModel);
		}

		[HttpGet]
		public ActionResult RemoveStreet()
		{
			RemoveStreetViewModel removeStreetViewModel = new RemoveStreetViewModel();

			foreach (string unusedStreet in _GetUnuesdStreets())
				removeStreetViewModel.UnusedStreets.Add(unusedStreet);

			return View(removeStreetViewModel);
		}
		[HttpPost]
		public ActionResult RemoveStreet(RemoveStreetViewModel removeStreetViewModel)
		{
			if (ModelState.IsValid)
			{
				_deliveryRepository.RemoveStreet(removeStreetViewModel.StreetToRemove);
				return Redirect(Url.Action("ManageDeliveryZones", "Delivery"));
			}
			else
				return View(removeStreetViewModel);
		}

		[HttpGet]
		public ActionResult AddDeliveryZone()
		{
			return View(new DeliveryZoneViewModel(_deliveryRepository.GetUnmappedStreets().Select(unmappedStreet => new KeyValuePair<string, bool>(unmappedStreet, false)))
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
						UniqueConstraintException uniqueConstraintException = (aggregatedException as UniqueConstraintException);

						if (uniqueConstraintException != null && string.Equals(uniqueConstraintException.ConstraintName, "http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd:UniqueDeliveryZoneNames", StringComparison.Ordinal))
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueConstraintException.ConflictingValue));
					}
				}
			}

			deliveryZoneViewModel.AvailableStreets.Clear();
			foreach (string unmappedStreet in _deliveryRepository.GetUnmappedStreets())
				deliveryZoneViewModel.AvailableStreets.Add(new KeyValuePair<string, bool>(unmappedStreet, Request.Form.AllKeys.Contains("checkbox " + unmappedStreet)));

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
					DeliveryZoneViewModel deliveryZoneViewModel = new DeliveryZoneViewModel(deliveryZoneFound.Streets.Select(street => new KeyValuePair<string, bool>(street, true))
																											 .Concat(_deliveryRepository.GetUnmappedStreets().Select(street => new KeyValuePair<string, bool>(street, false))))
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
						UniqueConstraintException uniqueConstraintException = (aggregatedException as UniqueConstraintException);

						if (uniqueConstraintException != null && string.Equals(uniqueConstraintException.ConstraintName, "http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd:UniqueDeliveryZoneNames", StringComparison.Ordinal))
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueConstraintException.ConflictingValue));
					}
				}
			}

			DeliveryZone deliveryZoneFound = _deliveryRepository.GetDeliveryZones().FirstOrDefault(deliveryZone => string.Equals(deliveryZoneViewModel.DeliveryZoneOldName, deliveryZone.Name, StringComparison.OrdinalIgnoreCase));
			if (deliveryZoneFound != null)
			{
				deliveryZoneViewModel.AvailableStreets.Clear();
				foreach (KeyValuePair<string, bool> street in deliveryZoneFound.Streets.Select(street => new KeyValuePair<string, bool>(street, true))
																					   .Concat(_deliveryRepository.GetUnmappedStreets().Select(street => new KeyValuePair<string, bool>(street, false))))
					deliveryZoneViewModel.AvailableStreets.Add(street);
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
						UniqueConstraintException uniqueConstraintException = (aggregatedException as UniqueConstraintException);

						if (uniqueConstraintException != null && string.Equals(uniqueConstraintException.ConstraintName, "http://storage.andrei15193.ro/public/schemas/Edesia/DeliveryMapping.xsd:UniqueDeliveryZoneNames", StringComparison.Ordinal))
							ModelState.AddModelError("DeliveryZoneName", string.Format(ErrorStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueConstraintException.ConflictingValue));
					}
				}

			return RemoveDeliveryZone();
		}

		private IEnumerable<string> _GetUnuesdStreets()
		{
			return _deliveryRepository.GetUnmappedStreets()
									  .Except(_applicationUserStore.GetAddresses().Select(address => address.Street), StringComparer.OrdinalIgnoreCase)
									  .OrderBy(street => street);
		}
		private DeliveryZone _GetDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			return new DeliveryZone(deliveryZoneViewModel.DeliveryZoneName,
									Colour.Parse(deliveryZoneViewModel.DeliveryZoneColour),
									Request.Form.Keys.Cast<string>()
													 .Where(inputName => inputName.StartsWith("checkbox "))
													 .Select(inputName => inputName.Substring(9)));

		}

		private readonly IApplicationUserStore _applicationUserStore = (IApplicationUserStore)MvcApplication.DependencyContainer["applicationUserStore"];
		private readonly IDeliveryRepository _deliveryRepository = (IDeliveryRepository)MvcApplication.DependencyContainer["deliveryRepository"];
	}
}