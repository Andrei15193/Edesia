using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
namespace Andrei15193.Edesia.ViewModels
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
		[EmailAddress, Required, Display(Name = "Adresa de E-Mail", Prompt = "nume@site.ro")]
		public string EMail
		{
			get;
			set;
		}
		[EmailAddress, Required, Display(Prompt = "reintroduceți adresa de e-mail (verificare)")]
		public string EMailCopy
		{
			get;
			set;
		}
		[Password, Required, Display(Name = "Parola", Prompt = "nu se va vedea")]
		public string Password
		{
			get;
			set;
		}
		[Password, Required, Display(Prompt = "reintroduceți parola (verificare)")]
		public string PasswordCopy
		{
			get;
			set;
		}
	}
}