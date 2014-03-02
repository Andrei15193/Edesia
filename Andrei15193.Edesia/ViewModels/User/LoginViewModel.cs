using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.ApplicationResources.Language;
using Andrei15193.Edesia.Attributes;
namespace Andrei15193.Edesia.ViewModels.User
{
	public class LoginViewModel
	{
		[EmailAddress, Required, Display(Name = "EMailDisplayName", Prompt = "EMailPrompt", ResourceType = typeof(LanguageResource))]
		public string Email
		{
			get;
			set;
		}
		public static string EmailPlaceholder = "";
		[Password, Required, MinLength(6), Display(Name = "PasswordDisplayName", Prompt = "PasswordPrompt", ResourceType = typeof(LanguageResource))]
		public string Password
		{
			get;
			set;
		}
		public static string PasswordPlaceholder = "";
	}
}