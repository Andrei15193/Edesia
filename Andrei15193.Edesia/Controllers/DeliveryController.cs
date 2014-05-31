using System.Web.Mvc;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Controllers
{
	[Authorize, Role(typeof(Administrator))]
	public class DeliveryController
		: ApplicationController
	{
		[HttpGet]
		public ActionResult Default()
		{
			return View();
		}
	}
}