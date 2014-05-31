using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Collections;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.ViewModels.Street;
namespace Andrei15193.Edesia.Controllers
{
	public class StreetController
		: ApplicationController
	{
		[ChildActionOnly]
		public ActionResult Unmapped()
		{
			IEnumerable<string> unusedStreets = new SortedSet<string>(_deliveryRepository.GetUnmappedStreets().Except(_orderRepository.GetUsedStreets()), StringComparer.Ordinal);

			return View(_deliveryRepository.GetUnmappedStreets()
										   .Select(street => KeyValuePair.Create(street, unusedStreets.Contains(street))));
		}

		[HttpGet, Authorize, Role(typeof(Administrator))]
		public ActionResult Add()
		{
			return View();
		}
		[HttpPost, Authorize, Role(typeof(Administrator))]
		public ActionResult Add(AddStreetViewModel addStreetViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_deliveryRepository.AddStreet(addStreetViewModel.Street);
					return Redirect(Url.Action("Default", "Delivery"));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueStreetException uniqueStreetException = aggregatedException as UniqueStreetException;

						if (uniqueStreetException != null)
							ModelState.AddModelError("Street", string.Format(StreetControllerStrings.StreetTextBox_InvalidDuplicateValue_Format, uniqueStreetException.ConflictingValue));
					}

					return View(addStreetViewModel);
				}
			}
			else
				return View(addStreetViewModel);
		}

		[HttpGet, Authorize, Role(typeof(Administrator))]
		public ActionResult Remove(string street)
		{
			if (street != null)
				_deliveryRepository.RemoveStreet(street);

			return RedirectToAction("Default", "Delivery");
		}

		private readonly IOrderProvider _orderRepository = (IOrderProvider)MvcApplication.DependencyContainer["orderRepository"];
		private readonly IDeliveryZoneRepository _deliveryRepository = (IDeliveryZoneRepository)MvcApplication.DependencyContainer["deliveryRepository"];
	}
}