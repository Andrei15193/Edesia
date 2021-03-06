﻿using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Andrei15193.DependencyInjection;
using Andrei15193.DependencyInjection.Configuration;
namespace Andrei15193.Edesia
{
	public class MvcApplication
		: HttpApplication
	{
		public const string DateTimeSerializationFormat = "yyyy-MM-dd\\THH:mm:ss.FFFFFFFzzz";
		public static T[] GetEmptyAray<T>()
		{
			object emptyArray;
			if (_emptyArrays.TryGetValue(typeof(T).FullName, out emptyArray))
				return (T[])emptyArray;
			emptyArray = new T[0];
			_emptyArrays.Add(typeof(T).FullName, emptyArray);
			return (T[])emptyArray;
		}

		internal static DependencyContainer DependencyContainer
		{
			get
			{
				return _dependencyContainer;
			}
		}
		internal const string AzureConnectionStringFormat = "SQLAZURECONNSTR_{0}";

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			_dependencyContainer.CreateAllSingletons();
		}

		protected void Application_BeginRequest()
		{
			CultureInfo culture = new CultureInfo("ro-RO");

			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		private static IDictionary<string, object> _emptyArrays = new Dictionary<string, object>();
		private static readonly DependencyContainer _dependencyContainer = new DependencyContainer((DependencyInjectionConfigurationSection)WebConfigurationManager.GetSection("DependencyInjection"));
	}
}