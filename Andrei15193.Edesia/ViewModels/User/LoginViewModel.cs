using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.User
{
	public sealed class LoginViewModel
	{
		[LocalizedEMailAddress(UserControllerKey.EMailTextBox_InvalidValue, typeof(UserControllerStrings))]
		[LocalizedRequired(UserControllerKey.EMailTextBox_MissingValue, typeof(UserControllerStrings))]
		[Display(Name = UserControllerKey.EMailTextBox_DisplayName, Prompt = UserControllerKey.EMailTextBox_Hint, ResourceType = typeof(UserControllerStrings))]
		public string EMailAddress
		{
			get;
			set;
		}
		[Password]
		[LocalizedRequired(UserControllerKey.PasswordInput_MissingValue, typeof(UserControllerStrings))]
		[Display(Name = UserControllerKey.PasswordInput_DisplayName, Prompt = UserControllerKey.PasswordInput_Hint, ResourceType = typeof(UserControllerStrings))]
		public string Password
		{
			get;
			set;
		}
	}
}