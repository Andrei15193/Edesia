using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.ViewModels.User
{
	public sealed class RegisterViewModel
		: IValidatableObject
	{
		#region IValidatableObject Members
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (!string.Equals(EMailAddress, EMailAddressCopy, StringComparison.Ordinal))
				yield return new ValidationResult(UserControllerStrings.EMailTextBox_InvalidVerificationValue, new[] { "EMailAddressCopy" });
			if (!string.Equals(Password, PasswordCopy, StringComparison.Ordinal))
				yield return new ValidationResult(UserControllerStrings.PasswordInput_InvalidVerificationValue, new[] { "PasswordCopy" });
		}
		#endregion
		[LocalizedRequired(UserControllerKey.FirstNameTextBox_MissingValue, typeof(UserControllerStrings), AllowEmptyStrings = false)]
		[LocalizedRegularExpression(@"\s*\w+([ \-]\w+)*\s*", UserControllerKey.FirstNameTextBox_InvalidValue, typeof(UserControllerStrings))]
		[Display(Name = "FirstNameTextBox_DisplayName", Prompt = "FirstNameTextBox_Hint", ResourceType = typeof(UserControllerStrings))]
		public string FirstName
		{
			get;
			set;
		}
		[LocalizedRequired(UserControllerKey.LastNameTextBox_MissingValue, typeof(UserControllerStrings), AllowEmptyStrings = false)]
		[LocalizedRegularExpression(@"\s*\w+([ \-]\w+)*\s*", UserControllerKey.LastNameTextBox_InvalidValue, typeof(UserControllerStrings))]
		[Display(Name = UserControllerKey.LastNameTextBox_DisplayName, Prompt = UserControllerKey.LastNameTextBox_Hint, ResourceType = typeof(UserControllerStrings))]
		public string LastName
		{
			get;
			set;
		}

		[LocalizedEMailAddress(UserControllerKey.EMailTextBox_InvalidValue, typeof(UserControllerStrings))]
		[LocalizedRequired(UserControllerKey.EMailTextBox_MissingValue, typeof(UserControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = UserControllerKey.EMailTextBox_DisplayName, Prompt = UserControllerKey.EMailTextBox_Hint, ResourceType = typeof(UserControllerStrings))]
		public string EMailAddress
		{
			get;
			set;
		}
		[LocalizedEMailAddress(UserControllerKey.EMailTextBox_InvalidValue, typeof(UserControllerStrings))]
		[LocalizedRequired(UserControllerKey.EMailTextBox_MissingValue, typeof(UserControllerStrings))]
		[Display(Name = UserControllerKey.EMailVerificationTextBox_DisplayName, Prompt = UserControllerKey.EMailVerificationTextBox_Hint, ResourceType = typeof(UserControllerStrings))]
		public string EMailAddressCopy
		{
			get;
			set;
		}

		[LocalizedRequired(UserControllerKey.PasswordInput_MissingValue, typeof(UserControllerStrings), AllowEmptyStrings = false)]
		[Password]
		[Display(Name = UserControllerKey.PasswordInput_DisplayName, Prompt = UserControllerKey.PasswordInput_Hint, ResourceType = typeof(UserControllerStrings))]
		public string Password
		{
			get;
			set;
		}
		[Password]
		[LocalizedRequired(UserControllerKey.PasswordInput_MissingValue, typeof(UserControllerStrings), AllowEmptyStrings = false)]
		[Display(Name = UserControllerKey.PasswordVerificationInput_DisplayName, Prompt = UserControllerKey.PasswordVerificationInput_Hint, ResourceType = typeof(UserControllerStrings))]
		public string PasswordCopy
		{
			get;
			set;
		}
	}
}