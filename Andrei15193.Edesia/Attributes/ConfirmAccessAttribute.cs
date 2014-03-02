using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Extensions;
namespace Andrei15193.Edesia.Attributes
{
	public class ConfirmAccessAttribute
		: FilterAttribute, IAuthorizationFilter
	{
		public ConfirmAccessAttribute()
			: this(null)
		{
		}
		public ConfirmAccessAttribute(params string[] roles)
		{
			_roles = roles;
		}
		#region IAuthorizationFilter Members
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			_authorizeAttribute.OnAuthorization(filterContext);
			if (filterContext.HttpContext.User.Identity.IsAuthenticated
				&& _roles != null
				&& !_roles.Intersect(filterContext.HttpContext.GetApplicationUser().Roles).Any())
				filterContext.Result = new RedirectResult("/");
		}
		#endregion

		private readonly IReadOnlyCollection<string> _roles;
		private readonly AuthorizeAttribute _authorizeAttribute = new AuthorizeAttribute();
	}
}