using System.Web.Mvc;
namespace Andrei15193.Edesia.Controllers
{
	public class HomeController
		: ApplicationController
	{
		public ActionResult Default()
		{
			return View();
		}
	}
}