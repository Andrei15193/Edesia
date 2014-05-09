using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Settings;
using Andrei15193.Edesia.ViewModels.User;
using Andrei15193.Edesia.Xml.Validation;
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
				if (_userStore.ClearRegistrationKey(email, key))
					return View("_Notice", new Notice(RegisterViewStrings.ViewTitle, null, NoticeStrings.Registration_Completed_Paragraph1));
				else
					return View("_Notice", new Notice(RegisterViewStrings.ViewTitle, null, NoticeStrings.Registration_TokenExpired_Paragraph1, NoticeStrings.Registration_TokenExpired_Paragraph2));
		}
		[HttpPost]
		public ActionResult Register(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid)
				try
				{
					string registrationKey = _GenerateRegistrationKey();

					_userStore.AddApplicationUser(new ApplicationUser(registerViewModel.EMailAddress, registerViewModel.FirstName, registerViewModel.LastName, DateTime.Now),
												  registerViewModel.Password,
												  registrationKey);
					_SendRegistrationEMail(registerViewModel, registrationKey);

					return View("_Notice", new Notice(RegisterViewStrings.ViewTitle, null, NoticeStrings.Registration_ConfirmationMailSent_Paragraph1));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueConstraintException uniqueConstraintException = (aggregatedException as UniqueConstraintException);

						if (uniqueConstraintException != null && string.Equals(uniqueConstraintException.ConstraintName, "http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd:UniqueEmails", StringComparison.Ordinal))
							ModelState.AddModelError("EMailAddress", string.Format(ErrorStrings.EMailTextBox_DuplicateValue_Format, uniqueConstraintException.ConflictingValue));
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
				ApplicationUser applicationUser = _userStore.Find(loginViewModel.EMailAddress, loginViewModel.Password);

				if (applicationUser != null)
				{
					FormsAuthentication.SignOut();
					HttpCookie authenticationCookie = FormsAuthentication.GetAuthCookie(loginViewModel.EMailAddress, true);

					_userStore.SetAuthenticationToken(applicationUser, authenticationCookie.Value, AuthenticationTokenType.Key);
					Response.SetCookie(authenticationCookie);
					if (Url.IsLocalUrl(returnUrl))
						return Redirect(returnUrl);
					else
						return RedirectToAction("Default", "Home");
				}
				else
					ModelState.AddModelError("EMailAddress", ErrorStrings.CredentialControls_InvalidValues);
			}

			return View(loginViewModel);
		}

		[HttpGet, ConfirmAccess]
		public ActionResult Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				_userStore.ClearAuthenticationKey(User.Identity.Name);
				FormsAuthentication.SignOut();
			}
			Session.Abandon();
			return Redirect("/");
		}

		[HttpGet]
		public ActionResult ChangeLanguage(string languageId, string returnUrl)
		{
			if (languageId == null || returnUrl == null)
				return RedirectToAction("Default", "Home");

			Response.SetCookie(GetLanguageCookie(languageId));
			if (Url.IsLocalUrl(returnUrl))
				return Redirect(returnUrl);
			else
				return RedirectToAction("Default", "Home");
		}

		[ChildActionOnly]
		public ActionResult NavigationBar()
		{
			IList<NavigationBarAction> userActions = new List<NavigationBarAction>();

			if (User.Identity.IsAuthenticated)
				userActions.Add(new NavigationBarAction(NavigationViewStrings.LogoutButton_DisplayName, "Logout", "User", Icons.User));
			else
			{
				userActions.Add(new NavigationBarAction(NavigationViewStrings.LoginButton_DisplayName, "Login", "User", Icons.User));
				userActions.Add(new NavigationBarAction(NavigationViewStrings.RegisterButton_DisplayName, "Register", "User", Icons.New));
			}
			return View("_NavigationBar", userActions);
		}
		[ChildActionOnly]
		public ActionResult LanguageDropdown()
		{
			return View("_LanguageDropdown", new[] { new DisplayLanguage("Română", "RO", true) });
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
				Subject = "Edesia - " + RegisterViewStrings.ViewTitle,
				From = emailSettings.SenderMailAddress,
				To =
				{
					new MailAddress(registerViewModel.EMailAddress)
				},
				DeliveryNotificationOptions = DeliveryNotificationOptions.Never,
				IsBodyHtml = true,
				BodyEncoding = Encoding.UTF8,
				Body = string.Format(EMailNoticeStrings.Register_MailBody_Format,
									 new StringBuilder(Request.Url.Scheme).Append(Uri.SchemeDelimiter)
																		  .Append(Request.Url.Host)
																		  .Append(Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port)
																		  .Append("/User/Register")
																		  .ToString(),
									 Uri.EscapeDataString(registerViewModel.EMailAddress),
									 Uri.EscapeDataString(registrationKey))
			});
		}

		private readonly IApplicationUserStore _userStore = (IApplicationUserStore)MvcApplication.DependencyContainer["applicationUserStore"];
	}
}