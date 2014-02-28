using System;
using System.Web;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Extensions
{
	internal static class HttpContextBaseExtensions
	{
		internal static User GetUser(this HttpContextBase httpContextBase)
		{
			if (httpContextBase == null)
				throw new ArgumentNullException("httpContextBase");

			return (User)httpContextBase.Session[httpContextBase.User.Identity.Name];
		}
		internal static User GetUser(this HttpContextBase httpContextBase, string key)
		{
			if (httpContextBase == null)
				throw new ArgumentNullException("httpContextBase");
			if (key == null)
				throw new ArgumentNullException("key");

			return (User)httpContextBase.Session[key];
		}
		internal static void SetUser(this HttpContextBase httpContextBase, User user, string key)
		{
			if (httpContextBase == null)
				throw new ArgumentNullException("httpContextBase");
			if (user == null)
				throw new ArgumentNullException("user");
			if (key == null)
				throw new ArgumentNullException("key");

			httpContextBase.Session.Add(key, user);
		}
		internal static void SetUser(this HttpContextBase httpContextBase, User user)
		{
			if (httpContextBase == null)
				throw new ArgumentNullException("httpContextBase");
			if (user == null)
				throw new ArgumentNullException("user");

			httpContextBase.Session.Add(httpContextBase.User.Identity.Name, user);
		}
	}
}