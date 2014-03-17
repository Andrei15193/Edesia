using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
namespace Andrei15193.Edesia.ViewModels.User
{
	public class RegisterViewModel
		: IValidatableObject
	{
		#region IValidatableObject Members
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (!string.Equals(EMail, EMailCopy, StringComparison.Ordinal))
				yield return new ValidationResult(Resources.Strings.Error.EMailAddressesAreNotEqualMessage, new[] { "EMailCopy" });
			if (!string.Equals(Password, PasswordCopy, StringComparison.Ordinal))
				yield return new ValidationResult(Resources.Strings.Error.PasswordsAreNotEqualMessage, new[] { "PasswordCopy" });
		}
		#endregion
		[EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEMailAddress", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Required(AllowEmptyStrings = false, ErrorMessage = null, ErrorMessageResourceName = "MissingEMailMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Name = "EMailLabel", Prompt = "EMailPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string EMail
		{
			get;
			set;
		}
		[EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEMailAddress", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Required(AllowEmptyStrings = false, ErrorMessage = null, ErrorMessageResourceName = "MissingEMailMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Prompt = "EMailCopyPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string EMailCopy
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
		[Password]
		[Required(AllowEmptyStrings = true, ErrorMessage = null, ErrorMessageResourceName = "MissingPasswordMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Prompt = "PasswordCopyPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string PasswordCopy
		{
			get;
			set;
		}
	}
}