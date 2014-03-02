using System;
using System.Web;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Extensions
{
	internal static class HttpContextExtensions
	{
		internal static ApplicationUser GetApplicationUser(this HttpContext httpContext)
		{
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			return (ApplicationUser)httpContext.Session[httpContext.User.Identity.Name];
		}
		internal static void SetApplicationUser(this HttpContext httpContext, ApplicationUser applicationUser)
		{
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");
			if (applicationUser == null)
				throw new ArgumentNullException("user");

			httpContext.Session.Add(httpContext.User.Identity.Name, applicationUser);
		}
	}
}