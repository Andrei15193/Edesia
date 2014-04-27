using System;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.ViewModels.User
{
	public sealed class UserDetailsViewModel
	{
		public UserDetailsViewModel(ApplicationUser applicationUser)
		{
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");

			EMailAddress = applicationUser.EMailAddress;
			FirstName = applicationUser.FirstName;
			LastName = applicationUser.LastName;
			Street = applicationUser.Street;
		}

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
		[Required(AllowEmptyStrings = false, ErrorMessage = null, ErrorMessageResourceName = "MissingEMailAddressMessage", ErrorMessageResourceType = typeof(Resources.Strings.Error))]
		[Display(Name = "EMailAddressLabel", Prompt = "EMailAddressPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string EMailAddress
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
		[Display(Name = "PasswordLabel", Prompt = "PasswordCopyPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string PasswordCopy
		{
			get;
			set;
		}

		[Display(Name = "StreetLabel", Prompt = "StreetPlaceholder", ResourceType = typeof(Resources.Strings.View))]
		public string Street
		{
			get;
			set;
		}
	}
}