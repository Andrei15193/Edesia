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
		[EmailAddress, Required, Display(Name = "EMailDisplayName", Prompt = "EMailPrompt", ResourceType = typeof(Resources))]
		public string EMail
		{
			get;
			set;
		}
		[EmailAddress, Required, Display(Prompt = "EMailCopyPrompt", ResourceType = typeof(Resources))]
		public string EMailCopy
		{
			get;
			set;
		}
		[Password, Required, Display(Name = "PasswordDisplayName", Prompt = "PasswordPrompt", ResourceType = typeof(Resources))]
		public string Password
		{
			get;
			set;
		}
		[Password, Required, Display(Prompt = "PasswordCopyPrompt", ResourceType = typeof(Resources))]
		public string PasswordCopy
		{
			get;
			set;
		}
	}
}