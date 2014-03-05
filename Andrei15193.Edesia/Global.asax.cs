using System;
using System.Collections.Generic;
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
		public static T[] GetEmptyAray<T>()
		{
			object emptyArray;
			if (_emptyArrays.TryGetValue(typeof(T).FullName, out emptyArray))
				return (T[])emptyArray;
			emptyArray = new T[0];
			_emptyArrays.Add(typeof(T).FullName, emptyArray);
			return (T[])emptyArray;
		}

		protected void Application_Start()
		{
			Resources.Strings.DefaultLanguageId = EdesiaSettings.LocalizationStrings.DefaultLanguageId;
			foreach (ILocalizationConfigElement item in EdesiaSettings.LocalizationStrings)
				Resources.Strings.RegisterLanguageStrings(item);

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
		protected void Session_Start()
		{
			//new UserController().Login(User.Identity.Name, Context);
		}

		private static EdesiaConfigurationSection _edesiaSettings = (EdesiaConfigurationSection)WebConfigurationManager.GetSection("EdesiaSettings");
		private static IDictionary<string, object> _emptyArrays = new Dictionary<string, object>();
	}
}