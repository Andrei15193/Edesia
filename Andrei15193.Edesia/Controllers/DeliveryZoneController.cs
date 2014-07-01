using System;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.DeliveryZone;
namespace Andrei15193.Edesia.Controllers
{
	[Authorize, Role(typeof(Administrator))]
	public class DeliveryZoneController
		: ApplicationController
	{
		[ChildActionOnly]
		public ActionResult Default()
		{
			return View(_deliveryRepository.GetDeliveryZones());
		}

		[HttpGet]
		public ActionResult Add()
		{
			return View(new DeliveryZoneViewModel(_deliveryRepository.GetUnmappedStreets().Select(unmappedStreet => new AvailableStreet(unmappedStreet, false)),
												  _userRepository.GetEmployees()));
		}
		[HttpPost]
		public ActionResult Add(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.Add(_GetDeliveryZone(deliveryZoneViewModel));
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
				deliveryZoneViewModel.AvailableStreets.Add(new AvailableStreet(unmappedStreet, Request.Form.AllKeys.Contains("checkbox " + unmappedStreet)));

			foreach (Employee employee in _userRepository.GetEmployees())
				deliveryZoneViewModel.Employees.Add(employee);

			return View(deliveryZoneViewModel);
		}

		[HttpGet]
		public ActionResult Edit(string deliveryZone)
		{
			if (deliveryZone != null)
			{
				DeliveryZone deliveryZoneFound = _deliveryRepository.GetDeliveryZones().FirstOrDefault(storedDeiveryZone => string.Equals(deliveryZone, storedDeiveryZone.Name, StringComparison.OrdinalIgnoreCase));

				if (deliveryZoneFound != null)
					return View(new DeliveryZoneViewModel(deliveryZoneFound.Streets.Select(street => new AvailableStreet(street, true))
																				   .Concat(_deliveryRepository.GetUnmappedStreets()
																											  .Select(unmappedStreet => new AvailableStreet(unmappedStreet, false))),
														  _userRepository.GetEmployees())
					{
						DeliveryZoneName = deliveryZoneFound.Name,
						DeliveryZoneColour = deliveryZoneFound.Colour.ToString(),
						DeliveryZoneOldName = deliveryZone,
						SelectedEmployeeEMailAddress = (deliveryZoneFound.Assignee == null ? null : deliveryZoneFound.Assignee.EMailAddress),
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
					_deliveryRepository.Update(_GetDeliveryZone(deliveryZoneViewModel), deliveryZoneViewModel.DeliveryZoneOldName);
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

			DeliveryZone deliveryZone = _deliveryRepository.GetDeliveryZone(deliveryZoneViewModel.DeliveryZoneOldName);
			if (deliveryZone == null)
				return RedirectToAction("Default", "Delivery");

			deliveryZoneViewModel.AvailableStreets.Clear();
			foreach (AvailableStreet availableStreet in deliveryZone.Streets.Select(address => new AvailableStreet(address, true))
																				 .Concat(Request.Form.Keys.Cast<string>()
																										  .Where(inputName => inputName.StartsWith("checkbox "))
																										  .Select(inputName => new AvailableStreet(inputName.Substring(9), true)))
																				 .Concat(_deliveryRepository.GetUnmappedStreets().Select(unmappedStreet => new AvailableStreet(unmappedStreet, false))))
				deliveryZoneViewModel.AvailableStreets.Add(availableStreet);

			foreach (Employee employee in _userRepository.GetEmployees())
				deliveryZoneViewModel.Employees.Add(employee);

			return View(deliveryZoneViewModel);
		}

		[HttpGet]
		public ActionResult Remove(string deliveryZone)
		{
			if (deliveryZone != null)
			{
				DeliveryZone deliveryZoneToRemove = _deliveryRepository.GetDeliveryZone(Server.UrlDecode(deliveryZone));

				if (deliveryZoneToRemove != null)
					_deliveryRepository.Remove(deliveryZoneToRemove);
			}

			return RedirectToAction("Default", "Delivery");
		}

		private DeliveryZone _GetDeliveryZone(DeliveryZoneViewModel deliveryZoneViewModel)
		{
			DeliveryZone deliveryZone = new DeliveryZone(deliveryZoneViewModel.DeliveryZoneName,
														 Colour.Parse(deliveryZoneViewModel.DeliveryZoneColour),
														 Request.Form.Keys.Cast<string>()
																		  .Where(inputName => inputName.StartsWith("checkbox "))
																		  .Select(inputName => inputName.Substring(9)));

			if (!string.IsNullOrWhiteSpace(deliveryZoneViewModel.SelectedEmployeeEMailAddress))
				deliveryZone.Assignee = _userRepository.Find(deliveryZoneViewModel.SelectedEmployeeEMailAddress).TryGetRole<Employee>();

			return deliveryZone;
		}

		private readonly IDeliveryRepository _deliveryRepository = (IDeliveryRepository)MvcApplication.DependencyContainer["deliveryRepo"];
		private readonly IUserRepository _userRepository = (IUserRepository)MvcApplication.DependencyContainer["userRepository"];
	}
}