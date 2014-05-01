using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.User
{
	public sealed class LoginViewModel
	{
		[LocalizedEMailAddress(ErrorKey.EMailTextBox_InvalidValue)]
		[LocalizedRequired(ErrorKey.EMailTextBox_MissingValue)]
		[Display(Name = LoginViewKey.EMailTextBox_DisplayName, Prompt = LoginViewKey.EMailTextBox_Hint, ResourceType = typeof(LoginViewStrings))]
		public string EMailAddress
		{
			get;
			set;
		}
		[Password]
		[LocalizedRequired(ErrorKey.PasswordInput_MissingValue)]
		[Display(Name = LoginViewKey.PasswordInput_DisplayName, Prompt = LoginViewKey.PasswordInput_Hint, ResourceType = typeof(LoginViewStrings))]
		public string Password
		{
			get;
			set;
		}
	}
}