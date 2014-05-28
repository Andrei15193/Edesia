using System;
namespace Andrei15193.Edesia.Models
{
	public static class ApplicationUserExtensions
	{
		public static bool IsInRole<TApplicationUserRole>(this ApplicationUser applicationUser)
			where TApplicationUserRole : ApplicationUserRole
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");

			ApplicationUserRole applicationUserRole = (applicationUser as ApplicationUserRole);
			if (applicationUserRole == null)
				return false;

			return applicationUserRole.IsInRole<TApplicationUserRole>();
		}
		public static ApplicationUserRole TryGetRole<TApplicationUserRole>(this ApplicationUser applicationUser)
			where TApplicationUserRole : ApplicationUserRole
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");

			ApplicationUserRole applicationUserRole = (applicationUser as ApplicationUserRole);
			if (applicationUserRole == null)
				return null;

			return applicationUserRole.TryGetRole<TApplicationUserRole>();
		}
	}
}