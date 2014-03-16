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
				yield return new ValidationResult("Adresele de e-mail trebuie să fie aceleași!");
			if (!string.Equals(Password, PasswordCopy, StringComparison.Ordinal))
				yield return new ValidationResult("Parolele trebuie să fie aceleași!");
		}
		#endregion
		[EmailAddress, Required, Display(Name = "EMailLabel", Prompt = "EMailPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string EMail
		{
			get;
			set;
		}
		[EmailAddress, Required, Display(Prompt = "EMailCopyPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string EMailCopy
		{
			get;
			set;
		}
		[Password, Required, Display(Name = "PasswordLabel", Prompt = "PasswordPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string Password
		{
			get;
			set;
		}
		[Password, Required, Display(Prompt = "PasswordCopyPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string PasswordCopy
		{
			get;
			set;
		}
	}
}