using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.DeliveryZone;
namespace Andrei15193.Edesia.Controllers
{
	public class DeliveryZoneController
		: ApplicationController
	{
		[ChildActionOnly]
		public ActionResult Default()
		{
			return View(_deliveryRepository.GetDeliveryZones(_applicationUserProvider));
		}

		[HttpGet]
		public ActionResult Add()
		{
			return View(new DeliveryZoneViewModel(_deliveryRepository.GetUnmappedStreets().Select(unmappedStreet => new KeyValuePair<string, bool>(unmappedStreet, false)),
												  _applicationUserProvider.GetEmployees())
						{
							SubmitButtonText = DeliveryZoneControllerStrings.AddDeliveryZoneButton_DisplayName
						});
		}
		[HttpPost]
		public ActionResult Add(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.AddDeliveryZone(_GetDeliveryZone(deliveryZoneViewModel));
					return Redirect(Url.Action("Default", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueDeliveryZoneNameException uniqueDeliveryZoneNameException = aggregatedException as UniqueDeliveryZoneNameException;

						if (uniqueDeliveryZoneNameException != null)
							ModelState.AddModelError("DeliveryZoneName", string.Format(DeliveryZoneControllerStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueDeliveryZoneNameException.ConflictingValue));
					}
				}
			}

			deliveryZoneViewModel.AvailableStreets.Clear();
			foreach (string unmappedStreet in _deliveryRepository.GetUnmappedStreets())
				deliveryZoneViewModel.AvailableStreets.Add(new KeyValuePair<string, bool>(unmappedStreet, Request.Form.AllKeys.Contains("checkbox " + unmappedStreet)));

			foreach (Employee employee in _applicationUserProvider.GetEmployees())
				deliveryZoneViewModel.Employees.Add(employee);

			return View(deliveryZoneViewModel);
		}

		[HttpGet]
		public ActionResult Edit(string deliveryZone)
		{
			if (deliveryZone != null)
			{
				DeliveryZone deliveryZoneFound = _deliveryRepository.GetDeliveryZones(_applicationUserProvider).FirstOrDefault(storedDeiveryZone => string.Equals(deliveryZone, storedDeiveryZone.Name, StringComparison.OrdinalIgnoreCase));

				if (deliveryZoneFound != null)
					return View(new DeliveryZoneViewModel(deliveryZoneFound.Streets.Select(street => new KeyValuePair<string, bool>(street, true))
																				   .Concat(_deliveryRepository.GetUnmappedStreets().Select(street => new KeyValuePair<string, bool>(street, false))),
														  _applicationUserProvider.GetEmployees())
					{
						DeliveryZoneName = deliveryZoneFound.Name,
						DeliveryZoneColour = deliveryZoneFound.Colour.ToString(),
						DeliveryZoneOldName = deliveryZone,
						SelectedEmployeeEMailAddress = (deliveryZoneFound.Assignee == null ? null : deliveryZoneFound.Assignee.EMailAddress),
						SubmitButtonText = DeliveryZoneControllerStrings.EditDeliveryZoneButton_DisplayName
					});
			}

			return View("Default", "Delivery");
		}
		[HttpPost]
		public ActionResult Edit(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.UpdateDeliveryZone(_GetDeliveryZone(deliveryZoneViewModel), deliveryZoneViewModel.DeliveryZoneOldName);
					return Redirect(Url.Action("Default", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueDeliveryZoneNameException uniqueDeliveryZoneNameException = aggregatedException as UniqueDeliveryZoneNameException;

						if (uniqueDeliveryZoneNameException != null)
							ModelState.AddModelError("DeliveryZoneName", string.Format(DeliveryZoneControllerStrings.DeliveryZoneNameTextBox_InvalidDuplicateValue_Format, uniqueDeliveryZoneNameException.ConflictingValue));
					}
				}
			}

			DeliveryZone deliveryZoneFound = _deliveryRepository.GetDeliveryZones(_applicationUserProvider)
																.FirstOrDefault(deliveryZone => string.Equals(deliveryZoneViewModel.DeliveryZoneOldName, deliveryZone.Name, StringComparison.OrdinalIgnoreCase));
			if (deliveryZoneFound == null)
				return RedirectToAction("Default", "Delivery");

			deliveryZoneViewModel.AvailableStreets.Clear();
			foreach (KeyValuePair<string, bool> street in deliveryZoneFound.Streets.Select(address => new KeyValuePair<string, bool>(address, true))
																				   .Concat(Request.Form.Keys.Cast<string>()
																											.Where(inputName => inputName.StartsWith("checkbox "))
																											.Select(inputName => new KeyValuePair<string, bool>(inputName.Substring(9), true)))
																				   .Concat(_deliveryRepository.GetUnmappedStreets().Select(address => new KeyValuePair<string, bool>(address, false))))
				deliveryZoneViewModel.AvailableStreets.Add(street);

			foreach (Employee employee in _applicationUserProvider.GetEmployees())
				deliveryZoneViewModel.Employees.Add(employee);

			deliveryZoneViewModel.SubmitButtonText = DeliveryZoneControllerStrings.EditDeliveryZoneButton_DisplayName;
			return View(deliveryZoneViewModel);
		}

		[HttpGet]
		public ActionResult Remove(string deliveryZone)
		{
			if (deliveryZone != null)
				_deliveryRepository.RemoveDeliveryZone(deliveryZone);

			return RedirectToAction("Default", "Delivery");
		}

		private DeliveryZone _GetDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			return new DeliveryZone(deliveryZoneViewModel.DeliveryZoneName,
									Colour.Parse(deliveryZoneViewModel.DeliveryZoneColour),
									Request.Form.Keys.Cast<string>()
													 .Where(inputName => inputName.StartsWith("checkbox "))
													 .Select(inputName => inputName.Substring(9)))
				{
					Assignee = _applicationUserProvider.GetEmployees().FirstOrDefault(employee => string.Equals(employee.EMailAddress, deliveryZoneViewModel.SelectedEmployeeEMailAddress, StringComparison.Ordinal))
				};
		}

		private readonly IDeliveryZoneRepository _deliveryRepository = (IDeliveryZoneRepository)MvcApplication.DependencyContainer["deliveryRepository"];
		private readonly IApplicationUserProvider _applicationUserProvider = (IApplicationUserProvider)MvcApplication.DependencyContainer["applicationUserRepository"];
	}
}