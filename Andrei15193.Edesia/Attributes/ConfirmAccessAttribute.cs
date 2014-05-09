using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Controllers;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Attributes
{
	public class ConfirmAccessAttribute
		: FilterAttribute, IAuthorizationFilter
	{
		public ConfirmAccessAttribute()
			: this(null)
		{
		}
		public ConfirmAccessAttribute(params Type[] roles)
		{
			_roles = roles;
		}

		#region IAuthorizationFilter Members
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			_authorizeAttribute.OnAuthorization(filterContext);

			if (filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				ApplicationUserRole applicationUserRole = ApplicationController.GetApplicationUser(filterContext.HttpContext) as ApplicationUserRole;

				if (_roles != null && (applicationUserRole == null || _roles.All(role => !applicationUserRole.IsInRole(role))))
					filterContext.Result = new RedirectResult("/");
			}
		}
		#endregion

		private readonly IReadOnlyCollection<Type> _roles;
		private readonly AuthorizeAttribute _authorizeAttribute = new AuthorizeAttribute();
	}
}