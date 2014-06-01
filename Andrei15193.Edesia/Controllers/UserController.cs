using System;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Settings;
using Andrei15193.Edesia.ViewModels.User;
namespace Andrei15193.Edesia.Controllers
{
	public class UserController
		: ApplicationController
	{
		[HttpGet]
		public ActionResult Register(string email, string key)
		{
			if (email == null || key == null)
				return View();
			else
				if (_applicationUserRepository.ClearRegistrationKey(email, key))
					return View("_Notice", new Notice(UserControllerStrings.RegisterViewTitle, null, UserControllerStrings.Registration_Completed_Paragraph1));
				else
					return View("_Notice", new Notice(UserControllerStrings.RegisterViewTitle, null, UserControllerStrings.Registration_TokenExpired_Paragraph1, UserControllerStrings.Registration_TokenExpired_Paragraph2));
		}
		[HttpPost]
		public ActionResult Register(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid)
				try
				{
					string registrationKey = _GenerateRegistrationKey();

					_applicationUserRepository.AddApplicationUser(new ApplicationUser(registerViewModel.EMailAddress, registerViewModel.FirstName, registerViewModel.LastName, DateTime.Now),
																  registerViewModel.Password,
																  registrationKey);
					_SendRegistrationEMail(registerViewModel, registrationKey);

					return View("_Notice", new Notice(UserControllerStrings.RegisterViewTitle, null, UserControllerStrings.Registration_ConfirmationMailSent_Paragraph1));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueEMailAddressException uniqueEMailAddressException = aggregatedException as UniqueEMailAddressException;

						if (uniqueEMailAddressException != null)
							ModelState.AddModelError("EMailAddress", string.Format(UserControllerStrings.EMailTextBox_DuplicateValue_Format, uniqueEMailAddressException.ConflictingValue));
					}

					return View(registerViewModel);
				}
			else
				return View();
		}

		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser applicationUser = _applicationUserRepository.Find(loginViewModel.EMailAddress, loginViewModel.Password);

				if (applicationUser != null)
				{
					FormsAuthentication.SignOut();
					HttpCookie authenticationCookie = FormsAuthentication.GetAuthCookie(loginViewModel.EMailAddress, true);

					_applicationUserRepository.SetAuthenticationToken(applicationUser, authenticationCookie.Value, AuthenticationTokenType.Key);
					Response.SetCookie(authenticationCookie);
					if (Url.IsLocalUrl(returnUrl))
						return Redirect(returnUrl);
					else
						return RedirectToAction("Default", "Product");
				}
				else
					ModelState.AddModelError("EMailAddress", UserControllerStrings.CredentialControls_InvalidValues);
			}

			return View(loginViewModel);
		}

		[HttpGet, Authorize]
		public ActionResult Logout()
		{
			if (User != null)
			{
				_applicationUserRepository.ClearAuthenticationKey(User.EMailAddress);
				FormsAuthentication.SignOut();
				Session.Abandon();
			}

			return RedirectToAction("Default", "Product");
		}

		[Authorize, Role(typeof(Administrator))]
		public ActionResult PromoteToAdmin(string eMail)
		{
			if (!string.IsNullOrWhiteSpace(eMail))
				_applicationUserRepository.EnrollAdministrator(Server.UrlDecode(eMail));

			return RedirectToAction("Default", "Product");
		}
		[Authorize, Role(typeof(Administrator))]
		public ActionResult PromoteToEmploye(string eMail, double transportCapacity)
		{
			if (!string.IsNullOrWhiteSpace(eMail))
				_applicationUserRepository.EnrollEmployee(Server.UrlDecode(eMail), transportCapacity);

			return RedirectToAction("Default", "Product");
		}
		
		private string _GenerateRegistrationKey()
		{
			Random random = new Random((int)(DateTime.Now.TimeOfDay.TotalMilliseconds));
			IRegistrationSettings registrationSettings = (IRegistrationSettings)MvcApplication.DependencyContainer["registrationSettings"];
			StringBuilder registrationKey = new StringBuilder(registrationSettings.RegistrationKeyLength);

			Func<int, int> _compute = (number =>
				{
					while (number / 10 > 0)
					{
						int sum = 0;
						do
						{
							sum += number % 10;
							number /= 10;
						} while (number > 0);
						number = sum;
					}
					return number;
				});

			for (int registrationKeyCharacterCount = 0; registrationKeyCharacterCount < registrationKey.Capacity; registrationKeyCharacterCount++)
				registrationKey.Append(_compute(random.Next(10000)));

			return registrationKey.ToString();
		}
		private void _SendRegistrationEMail(RegisterViewModel registerViewModel, string registrationKey)
		{
			IEMailSettings emailSettings = (IEMailSettings)MvcApplication.DependencyContainer["eMailSettings"];

			new SmtpClient
			{
				Host = emailSettings.SmtpHost,
				Port = emailSettings.SmtpPort,
				EnableSsl = true,
				Credentials = emailSettings.Credentials
			}.Send(new MailMessage
			{
				Subject = "Edesia - " + UserControllerStrings.RegisterViewTitle,
				From = emailSettings.SenderMailAddress,
				To =
				{
					new MailAddress(registerViewModel.EMailAddress)
				},
				DeliveryNotificationOptions = DeliveryNotificationOptions.Never,
				IsBodyHtml = true,
				BodyEncoding = Encoding.UTF8,
				Body = string.Format(UserControllerStrings.Register_MailBody_Format,
									 new StringBuilder(Request.Url.Scheme).Append(Uri.SchemeDelimiter)
																		  .Append(Request.Url.Host)
																		  .Append(Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port)
																		  .Append("/User/Register")
																		  .ToString(),
									 Uri.EscapeDataString(registerViewModel.EMailAddress),
									 Uri.EscapeDataString(registrationKey))
			});
		}

		private readonly IApplicationUserRepository _applicationUserRepository = (IApplicationUserRepository)MvcApplication.DependencyContainer["applicationUserRepository"];
	}
}