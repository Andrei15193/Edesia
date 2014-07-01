using System;
using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
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
			return View(_deliveryRepository.GetUnmappedStreets());
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
					_deliveryRepository.Add(addStreetViewModel.Street);
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
			{
				string decodedStreet = Server.UrlDecode(street);

				if (!string.IsNullOrWhiteSpace(decodedStreet))
					_deliveryRepository.Remove(decodedStreet);
			}

			return RedirectToAction("Default", "Delivery");
		}

		private readonly IDeliveryRepository _deliveryRepository = (IDeliveryRepository)MvcApplication.DependencyContainer["deliveryRepo"];
	}
}