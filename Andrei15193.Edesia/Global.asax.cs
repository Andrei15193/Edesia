using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia
{
	public class MvcApplication
		: HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
		protected void Session_Start()
		{
			Session["shoppingCart"] = new ShoppingCart();
		}
	}
}