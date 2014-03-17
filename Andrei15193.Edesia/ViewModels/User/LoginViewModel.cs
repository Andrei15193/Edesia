using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
namespace Andrei15193.Edesia.ViewModels.User
{
	public class LoginViewModel
	{
		[EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEMailAddress", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Required(AllowEmptyStrings = false, ErrorMessage = null, ErrorMessageResourceName = "MissingEMailMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Name = "EMailLabel", Prompt = "EMailPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string EMail
		{
			get;
			set;
		}
		[Password]
		[Required(AllowEmptyStrings = true, ErrorMessage = null, ErrorMessageResourceName = "MissingPasswordMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Name = "PasswordLabel", Prompt = "PasswordPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string Password
		{
			get;
			set;
		}
	}
}