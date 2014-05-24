﻿using System;
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
	}
}