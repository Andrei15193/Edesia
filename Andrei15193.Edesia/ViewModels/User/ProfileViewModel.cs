using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Andrei15193.Edesia.ApplicationResources.Language;
using Andrei15193.Edesia.Attributes;
namespace Andrei15193.Edesia.ViewModels.User
{
	public class ProfileViewModel
	{
		public ProfileViewModel(IEnumerable<string> roles = null)
		{
			if (roles == null)
				_roles = MvcApplication.GetEmptyAray<string>();
			else
				_roles = roles.ToList();
		}

		[EmailAddress, Required, Display(Name = "EMailDisplayName", Prompt = "EMailPrompt", ResourceType = typeof(LanguageResource))]
		public string EMail
		{
			get;
			set;
		}
		[Password, Required, Display(Name = "PasswordDisplayName", Prompt = "PasswordPrompt", ResourceType = typeof(LanguageResource))]
		public string Password
		{
			get;
			set;
		}
		[Password, Required, Display(Name = "PasswordDisplayName", Prompt = "PasswordCopyPrompt", ResourceType = typeof(LanguageResource))]
		public string PasswordCopy
		{
			get;
			set;
		}
		[Display(Name = "RolesDisplayName", ResourceType = typeof(LanguageResource))]
		public IReadOnlyCollection<string> Roles
		{
			get
			{
				return _roles;
			}
		}

		private readonly IReadOnlyCollection<string> _roles;
	}
}