using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.Extensions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.Settings;
using Andrei15193.Edesia.ViewModels.User;
using Andrei15193.Edesia.Xml.Validation;
namespace Andrei15193.Edesia.Controllers
{
	public class UserController
		: ApplicationController
	{
		[HttpGet, ConfirmAccess]
		public ActionResult Details()
		{
			ApplicationUser applicationUser = HttpContext.GetApplicationUser();
			return View(new ProfileViewModel(applicationUser.Roles)
			{
				EMail = applicationUser.EMail
			});
		}

		[HttpGet]
		public ActionResult Register(string email, string key)
		{
			if (email == null || key == null)
				return View();
			else
				if (_userStore.ClearRegistrationKey(email, key))
					return View("_Notice", new Notice(Resources.Strings.View.RegisterLabel, null, Resources.Strings.View.RegistrationCompleteNoticeParagraph1));
				else
					return View("_Notice", new Notice(Resources.Strings.View.RegisterLabel, null, Resources.Strings.Error.RegistrationTokenExpiredMessage));
		}
		[HttpPost]
		public ActionResult Register(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid)
				try
				{
					string registrationKey = _GenerateRegistrationKey();

					_userStore.AddApplicationUser(new ApplicationUser(registerViewModel.EMail, DateTime.Now)
						{
							Roles =
							{
								UserRoles.Client
							}
						}, registerViewModel.Password, registrationKey);
					_SendRegistrationEMail(registerViewModel, registrationKey);

					return View("_Notice", new Notice(Resources.Strings.View.RegisterLabel, null, Resources.Strings.View.RegisterMailSendNoticeParagraph1));
				}
				catch (AggregateException aggregateException)
				{
					foreach (Exception aggregatedException in aggregateException.InnerExceptions)
					{
						UniqueConstraintException uniqueConstraintException = (aggregatedException as UniqueConstraintException);

						if (uniqueConstraintException != null && string.Equals(uniqueConstraintException.ConstraintName, "http://storage.andrei15193.ro/public/schemas/Edesia/Membership.xsd:UniqueEmails", StringComparison.Ordinal))
							ModelState.AddModelError("EMail", string.Format(Resources.Strings.Error.DuplicateEMailMessageFormat, uniqueConstraintException.ConflictingValue));
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
				ApplicationUser applicationUser = _userStore.Find(loginViewModel.EMail, loginViewModel.Password);

				if (applicationUser != null)
				{
					FormsAuthentication.SignOut();
					HttpCookie authenticationCookie = FormsAuthentication.GetAuthCookie(loginViewModel.EMail, true);

					_userStore.SetAuthenticationToken(applicationUser, authenticationCookie.Value, AuthenticationTokenType.Key);
					Response.SetCookie(authenticationCookie);
					HttpContext.SetApplicationUser(applicationUser, loginViewModel.EMail);
					if (Url.IsLocalUrl(returnUrl))
						return Redirect(returnUrl);
					else
						return RedirectToAction("Default", "Home");
				}
				else
					ModelState.AddModelError(string.Empty, "Invalid username or password.");
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
		[NonAction]
		public void Login(string eMail, HttpContext context)
		{
			if (context.User.Identity.IsAuthenticated)
			{
				ApplicationUser applicationUser = _userStore.Find(eMail, context.Request.Cookies.Get(FormsAuthentication.FormsCookieName).Value, AuthenticationTokenType.Key);
				if (applicationUser != null)
					context.SetApplicationUser(applicationUser);
			}
		}
		public ActionResult GetNavigationBar()
		{
			IList<NavigationBarAction> userActions = new List<NavigationBarAction>();

			if (User.Identity.IsAuthenticated)
				userActions.Add(new NavigationBarAction(Resources.Strings.View.LogoutLabel, "Logout", "User", Icons.User));
			else
			{
				userActions.Add(new NavigationBarAction(Resources.Strings.View.LoginLabel, "Login", "User", Icons.User));
				userActions.Add(new NavigationBarAction(Resources.Strings.View.RegisterLabel, "Register", "User", Icons.New));
			}
			return View(userActions);
		}
		public ActionResult GetLanguageDropdown()
		{
			string selectedLanguageId = GetSelectedLanguageId();

			return View(Resources.Strings.LanguageSpecifications
										 .Select(availableLanguage => new DisplayLanguage(availableLanguage.LanguageDisplayName,
																						  availableLanguage.LanguageId,
																						  string.Equals(selectedLanguageId,
																										availableLanguage.LanguageId,
																										StringComparison.Ordinal))));
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
				Subject = "Edesia - " + Resources.Strings.View.RegisterLabel,
				From = emailSettings.SenderMailAddress,
				To =
				{
					new MailAddress(registerViewModel.EMail)
				},
				DeliveryNotificationOptions = DeliveryNotificationOptions.Never,
				IsBodyHtml = true,
				BodyEncoding = Encoding.UTF8,
				Body = string.Format(Resources.Strings.EMail.RegistrationBodyFormat,
									 new StringBuilder(Request.Url.Scheme).Append(Uri.SchemeDelimiter)
																		  .Append(Request.Url.Host)
																		  .Append(Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port)
																		  .Append("/User/Register")
																		  .ToString(),
									 Uri.EscapeDataString(registerViewModel.EMail),
									 Uri.EscapeDataString(registrationKey))
			});
		}

		private readonly IApplicationUserStore _userStore = (IApplicationUserStore)DependencyContainer["applicationUserStore"];
	}
}