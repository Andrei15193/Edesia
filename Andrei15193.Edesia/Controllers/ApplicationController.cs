using System;
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
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
			_applicationUser = null;
		}
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			_applicationUser = null;
		}

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

		new internal protected ApplicationUser User
		{
			get
			{
				if (_applicationUser == null && base.User.Identity.IsAuthenticated)
				{
					HttpCookie authenticationCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
					if (authenticationCookie != null)
						_applicationUser = ((IApplicationUserRepository)MvcApplication.DependencyContainer["applicationUserRepository"]).Find(base.User.Identity.Name, authenticationCookie.Value, AuthenticationTokenType.Key);
				}

				return _applicationUser;
			}
		}

		[ThreadStatic]
		private ApplicationUser _applicationUser = null;
	}
}