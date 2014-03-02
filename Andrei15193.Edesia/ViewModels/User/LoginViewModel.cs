using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Attributes;
namespace Andrei15193.Edesia.ViewModels.User
{
	public class LoginViewModel
	{
		[EmailAddress, Required, Display(Name = "Adresa de E-Mail", Prompt = "nume@site.ro")]
		public string Email
		{
			get;
			set;
		}
		public static string EmailPlaceholder = "";
		[Password, Required, MinLength(6), Display(Name = "Parola", Prompt = "nu se va vedea")]
		public string Password
		{
			get;
			set;
		}
		public static string PasswordPlaceholder = "";
	}
}