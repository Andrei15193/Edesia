using System.Web.Mvc;
namespace Andrei15193.Edesia.Controllers
{
    public class ErrorController
		: Controller
	{
		public ActionResult Forbidden()
		{
			return View();
		}
		public ActionResult NotFound()
		{
			return View();
		}
    }
}