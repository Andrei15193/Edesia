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
			if (!string.Equals(EMailAddress, EMailAddressCopy, StringComparison.Ordinal))
				yield return new ValidationResult(Resources.Strings.Error.EMailAddressesAreNotEqualMessage, new[] { "EMailAddressCopy" });
			if (!string.Equals(Password, PasswordCopy, StringComparison.Ordinal))
				yield return new ValidationResult(Resources.Strings.Error.PasswordsAreNotEqualMessage, new[] { "PasswordCopy" });
		}
		#endregion
		[Required(AllowEmptyStrings = false, ErrorMessage = null, ErrorMessageResourceName = "MissingFirstNameMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Name = "FirstNameLabel", Prompt = "FirstNamePlaceholder", ResourceType = typeof(Resources.Strings.View))]
		[RegularExpression(@"\s*\w+([ \-]\w+)*\s*", ErrorMessage = null, ErrorMessageResourceName = "InvalidFirstNameMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		public string FirstName
		{
			get;
			set;
		}
		[Required(AllowEmptyStrings = false, ErrorMessage = null, ErrorMessageResourceName = "MissingLastNameMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Name = "LastNameLabel", Prompt = "LastNamePlaceholder", ResourceType = typeof(Resources.Strings.View))]
		[RegularExpression(@"\s*\w+([ \-]\w+)*\s*", ErrorMessage = null, ErrorMessageResourceName = "InvalidLastNameMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		public string LastName
		{
			get;
			set;
		}

		[EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEMailAddressMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Required(AllowEmptyStrings = false, ErrorMessage = null, ErrorMessageResourceName = "MissingEMailMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Name = "EMailAddressLabel", Prompt = "EMailAddressPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string EMailAddress
		{
			get;
			set;
		}
		[EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEMailAddressMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Required(AllowEmptyStrings = false, ErrorMessage = null, ErrorMessageResourceName = "MissingEMailMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Prompt = "EMailAddressCopyPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string EMailAddressCopy
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