using System;
using System.Web;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Extensions
{
	internal static class HttpContextBaseExtensions
	{
		internal static ApplicationUser GetApplicationUser(this HttpContextBase httpContextBase)
		{
			if (httpContextBase == null)
				throw new ArgumentNullException("httpContextBase");

			return (ApplicationUser)httpContextBase.Session[httpContextBase.User.Identity.Name];
		}
		internal static ApplicationUser GetApplicationUser(this HttpContextBase httpContextBase, string key)
		{
			if (httpContextBase == null)
				throw new ArgumentNullException("httpContextBase");
			if (key == null)
				throw new ArgumentNullException("key");

			return (ApplicationUser)httpContextBase.Session[key];
		}
		internal static void SetApplicationUser(this HttpContextBase httpContextBase, ApplicationUser applicationUser, string key)
		{
			if (httpContextBase == null)
				throw new ArgumentNullException("httpContextBase");
			if (applicationUser == null)
				throw new ArgumentNullException("ApplicationUser");
			if (key == null)
				throw new ArgumentNullException("key");

			httpContextBase.Session.Add(key, applicationUser);
		}
		internal static void SetApplicationUser(this HttpContextBase httpContextBase, ApplicationUser applicationUser)
		{
			if (httpContextBase == null)
				throw new ArgumentNullException("httpContextBase");
			if (applicationUser == null)
				throw new ArgumentNullException("user");

			httpContextBase.Session.Add(httpContextBase.User.Identity.Name, applicationUser);
		}
	}
}