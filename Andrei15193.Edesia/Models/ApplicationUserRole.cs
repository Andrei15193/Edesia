using System;
using System.Collections.Generic;
using Andrei15193.Edesia.Collections;
namespace Andrei15193.Edesia.Models
{
	public class ApplicationUserRole
		: ApplicationUser
	{
		protected ApplicationUserRole(ApplicationUser applicationUser)
			: base(applicationUser)
		{
			_decoratedApplicationUser = applicationUser;
		}

		public override IReadOnlyCollection<string> Roles
		{
			get
			{
				if (_roles == null)
				{
					ISet<string> roles = new SortedSet<string>(StringComparer.Ordinal) { GetType().Name };

					ApplicationUserRole decoratedApplicationUserRole = _decoratedApplicationUser as ApplicationUserRole;

					if (decoratedApplicationUserRole != null)
						foreach (string decoratedRole in decoratedApplicationUserRole.Roles)
							roles.Add(decoratedRole);

					_roles = new ReadOnlyCollection<string>(roles);
				}

				return _roles;
			}
		}

		public bool IsInRole<TApplicationUserRole>()
			where TApplicationUserRole : ApplicationUserRole
		{
			ApplicationUserRole currentApplicationUserRole = this;

			while (currentApplicationUserRole != null && !(currentApplicationUserRole is TApplicationUserRole))
				currentApplicationUserRole = currentApplicationUserRole._decoratedApplicationUser as ApplicationUserRole;

			return (currentApplicationUserRole != null);
		}
		public bool IsInRole(Type applicationUserRole)
		{
			if (applicationUserRole == null)
				throw new ArgumentNullException("applicationUserRole");

			if (!typeof(ApplicationUserRole).IsAssignableFrom(applicationUserRole))
				throw new ArgumentException("Must be assignable to ApplicationUserRole", "applicationUserRole");

			ApplicationUserRole currentApplicationUserRole = this;

			while (currentApplicationUserRole != null && !(currentApplicationUserRole.GetType() == applicationUserRole))
				currentApplicationUserRole = currentApplicationUserRole._decoratedApplicationUser as ApplicationUserRole;

			return (currentApplicationUserRole != null);
		}

		public TApplicationUserRole TryGetRole<TApplicationUserRole>()
			where TApplicationUserRole : ApplicationUserRole
		{
			ApplicationUserRole currentApplicationUserRole = this;

			while (currentApplicationUserRole != null && !(currentApplicationUserRole is TApplicationUserRole))
				currentApplicationUserRole = currentApplicationUserRole._decoratedApplicationUser as ApplicationUserRole;

			return (TApplicationUserRole)currentApplicationUserRole;
		}
		public ApplicationUserRole TryGetRole(Type applicationUserRole)
		{
			if (applicationUserRole == null)
				throw new ArgumentNullException("applicationUserRole");

			if (!typeof(ApplicationUserRole).IsAssignableFrom(applicationUserRole))
				throw new ArgumentException("Must be assignable to ApplicationUserRole", "applicationUserRole");

			ApplicationUserRole currentApplicationUserRole = this;

			while (currentApplicationUserRole != null && !(currentApplicationUserRole.GetType() == applicationUserRole))
				currentApplicationUserRole = currentApplicationUserRole._decoratedApplicationUser as ApplicationUserRole;

			return currentApplicationUserRole;
		}

		private IReadOnlyCollection<string> _roles = null;
		private readonly ApplicationUser _decoratedApplicationUser;
	}
}