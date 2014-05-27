using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.User
{
	public sealed class LoginViewModel
	{
		[LocalizedEMailAddress(RegisterViewKey.EMailTextBox_InvalidValue)]
		[LocalizedRequired(RegisterViewKey.EMailTextBox_MissingValue)]
		[Display(Name = LoginViewKey.EMailTextBox_DisplayName, Prompt = LoginViewKey.EMailTextBox_Hint, ResourceType = typeof(LoginViewStrings))]
		public string EMailAddress
		{
			get;
			set;
		}
		[Password]
		[LocalizedRequired(RegisterViewKey.PasswordInput_MissingValue)]
		[Display(Name = LoginViewKey.PasswordInput_DisplayName, Prompt = LoginViewKey.PasswordInput_Hint, ResourceType = typeof(LoginViewStrings))]
		public string Password
		{
			get;
			set;
		}
	}
}