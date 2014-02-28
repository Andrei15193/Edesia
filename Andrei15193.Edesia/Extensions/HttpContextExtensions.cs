using System;
using System.Web;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Extensions
{
	internal static class HttpContextExtensions
	{
		internal static User GetUser(this HttpContext httpContext)
		{
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			return (User)httpContext.Session[httpContext.User.Identity.Name];
		}
		internal static void SetUser(this HttpContext httpContext, User user)
		{
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");
			if (user == null)
				throw new ArgumentNullException("user");

			httpContext.Session.Add(httpContext.User.Identity.Name, user);
		}
	}
}