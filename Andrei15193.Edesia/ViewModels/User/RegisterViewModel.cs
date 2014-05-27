﻿using System;
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
				yield return new ValidationResult(RegisterViewStrings.EMailTextBox_InvalidVerificationValue, new[] { "EMailAddressCopy" });
			if (!string.Equals(Password, PasswordCopy, StringComparison.Ordinal))
				yield return new ValidationResult(RegisterViewStrings.PasswordInput_InvalidVerificationValue, new[] { "PasswordCopy" });
		}
		#endregion
		[LocalizedRequired(RegisterViewKey.FirstNameTextBox_MissingValue, AllowEmptyStrings = false)]
		[LocalizedRegularExpression(@"\s*\w+([ \-]\w+)*\s*", RegisterViewKey.FirstNameTextBox_InvalidValue)]
		[Display(Name = "FirstNameTextBox_DisplayName", Prompt = "FirstNameTextBox_Hint", ResourceType = typeof(RegisterViewStrings))]
		public string FirstName
		{
			get;
			set;
		}
		[LocalizedRequired(RegisterViewKey.LastNameTextBox_MissingValue, AllowEmptyStrings = false)]
		[LocalizedRegularExpression(@"\s*\w+([ \-]\w+)*\s*", RegisterViewKey.LastNameTextBox_InvalidValue)]
		[Display(Name = RegisterViewKey.LastNameTextBox_DisplayName, Prompt = RegisterViewKey.LastNameTextBox_Hint, ResourceType = typeof(RegisterViewStrings))]
		public string LastName
		{
			get;
			set;
		}

		[LocalizedEMailAddress(RegisterViewKey.EMailTextBox_InvalidValue)]
		[LocalizedRequired(RegisterViewKey.EMailTextBox_MissingValue, AllowEmptyStrings = false)]
		[Display(Name = RegisterViewKey.EMailTextBox_DisplayName, Prompt = RegisterViewKey.EMailTextBox_Hint, ResourceType = typeof(RegisterViewStrings))]
		public string EMailAddress
		{
			get;
			set;
		}
		[LocalizedEMailAddress(RegisterViewKey.EMailTextBox_InvalidValue)]
		[LocalizedRequired(RegisterViewKey.EMailTextBox_MissingValue)]
		[Display(Name = RegisterViewKey.EMailVerificationTextBox_DisplayName, Prompt = RegisterViewKey.EMailVerificationTextBox_Hint, ResourceType = typeof(RegisterViewStrings))]
		public string EMailAddressCopy
		{
			get;
			set;
		}

		[LocalizedRequired(RegisterViewKey.PasswordInput_MissingValue, AllowEmptyStrings = false)]
		[Password]
		[Display(Name = RegisterViewKey.PasswordInput_DisplayName, Prompt = RegisterViewKey.PasswordInput_Hint, ResourceType = typeof(RegisterViewStrings))]
		public string Password
		{
			get;
			set;
		}
		[Password]
		[LocalizedRequired(RegisterViewKey.PasswordInput_MissingValue, AllowEmptyStrings = false)]
		[Display(Name = RegisterViewKey.PasswordVerificationInput_DisplayName, Prompt = RegisterViewKey.PasswordVerificationInput_Hint, ResourceType = typeof(RegisterViewStrings))]
		public string PasswordCopy
		{
			get;
			set;
		}
	}
}