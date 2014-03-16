using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
namespace Andrei15193.Edesia.ViewModels.User
{
	public class LoginViewModel
	{
		[EmailAddress, Required, Display(Name = "EMailLabel", Prompt = "EMailPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string Email
		{
			get;
			set;
		}
		[Password, Required, MinLength(6), Display(Name = "PasswordLabel", Prompt = "PasswordPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string Password
		{
			get;
			set;
		}
	}
}