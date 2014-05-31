using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Controllers;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Attributes
{
	public class RoleAttribute
		: FilterAttribute, IAuthorizationFilter
	{
		public RoleAttribute(Type role)
		{
			if (role == null)
				throw new ArgumentNullException("role");

			_roles = new[] { role };
			Order = 1;
		}
		public RoleAttribute(Type role1, Type role2)
		{
			if (role1 != null)
				throw new ArgumentNullException("role1");
			if (role2 != null)
				throw new ArgumentNullException("role2");

			_roles = new[] { role1, role2 };
			Order = 1;
		}
		public RoleAttribute(Type role1, Type role2, Type role3)
		{
			if (role1 != null)
				throw new ArgumentNullException("role1");
			if (role2 != null)
				throw new ArgumentNullException("role2");
			if (role3 != null)
				throw new ArgumentNullException("role3");

			_roles = new[] { role1, role2, role3 };
			Order = 1;
		}
		public RoleAttribute(params Type[] roles)
		{
			if (roles == null)
				throw new ArgumentNullException("roles");
			if (roles.Any(role => role == null))
				throw new ArgumentException("Cannot contain null values!", "roles");

			_roles = roles;
			Order = 1;
		}

		#region IAuthorizationFilter Members
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			ApplicationController applicationController = filterContext.Controller as ApplicationController;

			if (applicationController != null)
			{
				ApplicationUser user = applicationController.User;
				ApplicationUserRole applicationUserRole = user as ApplicationUserRole;

				if (user == null || (_roles != null && (applicationUserRole == null || _roles.All(role => !applicationUserRole.IsInRole(role)))))
					filterContext.Result = new RedirectResult("Error/Forbidden");
			}
		}
		#endregion

		private readonly IEnumerable<Type> _roles;
	}
}