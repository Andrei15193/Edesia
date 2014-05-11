using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Controllers
{
	public abstract class ApplicationController
		: Controller
	{
		internal static ApplicationUser GetApplicationUser(HttpContextBase httpContext)
		{
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			if (!httpContext.User.Identity.IsAuthenticated)
				return null;

			HttpCookie authenticationCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authenticationCookie == null)
				return null;

			return ((IApplicationUserRepository)MvcApplication.DependencyContainer["applicationUserRepository"]).Find(httpContext.User.Identity.Name, authenticationCookie.Value, AuthenticationTokenType.Key);
		}
		internal static ApplicationUser GetApplicationUser(HttpContext httpContext)
		{
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			if (!httpContext.User.Identity.IsAuthenticated)
				return null;

			HttpCookie authenticationCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authenticationCookie == null)
				return null;

			return ((IApplicationUserRepository)MvcApplication.DependencyContainer["applicationUserRepository"]).Find(httpContext.User.Identity.Name, authenticationCookie.Value, AuthenticationTokenType.Key);
		}

		protected override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			base.OnResultExecuting(filterContext);
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(GetSelectedLanguageId());
		}
		protected HttpCookie GetLanguageCookie(string languageId)
		{
			return new HttpCookie(_languageCookieName, languageId ?? "RO");
		}
		protected string GetSelectedLanguageId()
		{
			HttpCookie languageCookie = HttpContext.Request.Cookies[_languageCookieName];

			if (languageCookie != null)
				return languageCookie.Value;
			return "RO";
		}
		protected ApplicationUser ApplicationUser
		{
			get
			{
				if (!User.Identity.IsAuthenticated)
					return null;

				HttpCookie authenticationCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
				if (authenticationCookie == null)
					return null;

				return ((IApplicationUserRepository)MvcApplication.DependencyContainer["applicationUserRepository"]).Find(User.Identity.Name, authenticationCookie.Value, AuthenticationTokenType.Key);
			}
		}

		private const string _languageCookieName = "DisplayLanguage";
	}
}