using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Andrei15193.Edesia.Configuration;
using Andrei15193.Edesia.Controllers;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia
{
	public class MvcApplication
		: HttpApplication
	{
		public const string DateTimeSerializationFormat = "yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz";
		public static EdesiaConfigurationSection EdesiaSettings
		{
			get
			{
				return _edesiaSettings;
			}
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
		protected void Session_Start()
		{
			new UserController().Login(User.Identity.Name, Context);
		}

		private static EdesiaConfigurationSection _edesiaSettings = (EdesiaConfigurationSection)WebConfigurationManager.GetSection("EdesiaSettings");
	}
}