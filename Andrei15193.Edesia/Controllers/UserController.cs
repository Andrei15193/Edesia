//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using Andrei15193.Edesia.Attributes;
//using Andrei15193.Edesia.DataAccess;
//using Andrei15193.Edesia.Extensions;
//using Andrei15193.Edesia.Models;
//using Andrei15193.Edesia.ViewModels.User;
//namespace Andrei15193.Edesia.Controllers
//{
//	public class UserController
//		: ApplicationController
//	{
//		[ConfirmAccess(UserRoles.Client)]
//		public ActionResult UserProfile()
//		{
//			return View();
//		}
//		[HttpGet]
//		public ActionResult Register(string email, string key)
//		{
//			if (email == null || key == null)
//				return View();
//			else
//				if (_userStore.ClearRegistrationKey(email, key))
//					return View("_Notice", new Notice(Resources.RegisterLabel, null, "Contul dumneavoastră a fost creat cu success! Vă mulțumim ca ați ales să folosiți serviciile noastre!"));
//				else
//					return View("_Notice", new Notice(Resources.RegisterLabel, null, "Ne pare rău dar codul de înregistrare este prea vechi pentru a mai putea fi folosit. Trebuie sa vă reînregistrați."));
//		}
//		[HttpPost]
//		public ActionResult Register(RegisterViewModel registerViewModel)
//		{
//			if (ModelState.IsValid)
//			{
//				string registrationKey = _GenerateRegistrationKey();
//				_userStore.AddApplicationUser(new ApplicationUser(registerViewModel.EMail, DateTime.Now)
//					{
//						Roles =
//						{
//							UserRoles.Client
//						}
//					}, registerViewModel.Password, registrationKey);

//				new SmtpClient
//				{
//					Host = MvcApplication.EdesiaSettings.EmailSettings.SmtpHost,
//					Port = MvcApplication.EdesiaSettings.EmailSettings.SmtpPort,
//					EnableSsl = true,
//					Credentials = new NetworkCredential(MvcApplication.EdesiaSettings.EmailSettings.SmtpUsername,
//														MvcApplication.EdesiaSettings.EmailSettings.SmtpPassword)
//				}.Send(new MailMessage
//					{
//						Subject = "Edesia - " + Resources.RegisterLabel,
//						From = new MailAddress(MvcApplication.EdesiaSettings.EmailSettings.SenderEMailAddress, MvcApplication.EdesiaSettings.EmailSettings.SenderDisplayName),
//						To =
//						{
//							new MailAddress(registerViewModel.EMail)
//						},
//						DeliveryNotificationOptions = DeliveryNotificationOptions.Never,
//						IsBodyHtml = true,
//						BodyEncoding = Encoding.UTF8,
//						Body = string.Format(Resources.ConfirmationEMailBody,
//											 new StringBuilder(Request.Url.Scheme).Append(Uri.SchemeDelimiter)
//																				  .Append(Request.Url.Host)
//																				  .Append(Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port)
//																				  .Append("/User/Register")
//																				  .ToString(),
//											 Uri.EscapeDataString(registerViewModel.EMail),
//											 Uri.EscapeDataString(registrationKey))
//					});
//				return View("_Notice", new Notice(Resources.RegisterLabel, null, Resources.RegisterMessage));
//			}
//			return View();
//		}
//		[HttpGet, ConfirmAccess]
//		public ActionResult Logout()
//		{
//			if (User.Identity.IsAuthenticated)
//			{
//				_userStore.ClearAuthenticationKey(User.Identity.Name);
//				FormsAuthentication.SignOut();
//			}
//			Session.Abandon();
//			return Redirect("/");
//		}
//		[HttpGet]
//		public ActionResult Login()
//		{
//			return View();
//		}
//		[HttpPost]
//		public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
//		{
//			if (ModelState.IsValid)
//			{
//				ApplicationUser applicationUser = _userStore.Find(loginViewModel.Email, loginViewModel.Password);

//				if (applicationUser != null)
//				{
//					FormsAuthentication.SignOut();
//					HttpCookie authenticationCookie = FormsAuthentication.GetAuthCookie(loginViewModel.Email, true);

//					_userStore.SetAuthenticationToken(applicationUser, authenticationCookie.Value, AuthenticationTokenType.Key);
//					Response.SetCookie(authenticationCookie);
//					HttpContext.SetApplicationUser(applicationUser, loginViewModel.Email);
//					if (Url.IsLocalUrl(returnUrl))
//						return Redirect(returnUrl);
//					else
//						return RedirectToAction("Default", "Home");
//				}
//				else
//					ModelState.AddModelError(string.Empty, "Invalid username or password.");
//			}
//			return View(loginViewModel);
//		}
//		[HttpGet]
//		public ActionResult ChangeLanguage(string languageId, string returnUrl)
//		{
//			if (languageId == null || returnUrl == null)
//				return RedirectToAction("Default", "Home");

//			Response.SetCookie(GetLanguageCookie(languageId));
//			if (Url.IsLocalUrl(returnUrl))
//				return Redirect(returnUrl);
//			else
//				return RedirectToAction("Default", "Home");
//		}
//		[HttpGet, ConfirmAccess]
//		public ActionResult Profile()
//		{
//			ApplicationUser applicationUser = HttpContext.GetApplicationUser();
//			return View(new ProfileViewModel(applicationUser.Roles)
//				{
//					EMail = applicationUser.EMail
//				});
//		}
//		[NonAction]
//		public void Login(string eMail, HttpContext context)
//		{
//			if (context.User.Identity.IsAuthenticated)
//			{
//				ApplicationUser applicationUser = _userStore.Find(eMail, context.Request.Cookies.Get(FormsAuthentication.FormsCookieName).Value, AuthenticationTokenType.Key);
//				if (applicationUser != null)
//					context.SetApplicationUser(applicationUser);
//			}
//		}
//		public ActionResult GetNavigationBar()
//		{
//			IList<NavigationBarAction> userActions = new List<NavigationBarAction>();

//			if (User.Identity.IsAuthenticated)
//				userActions.Add(new NavigationBarAction(Resources.LogoutLabel, "Logout", "User", Icons.User));
//			else
//			{
//				userActions.Add(new NavigationBarAction(Resources.LoginLabel, "Login", "User", Icons.User));
//				userActions.Add(new NavigationBarAction(Resources.RegisterLabel, "Register", "User", Icons.New));
//			}
//			return View(userActions);
//		}
//		public ActionResult GetLanguageDropdown()
//		{
//			string selectedLanguageId = GetSelectedLanguageId();

//			return View(Resources.AvailableLanguages
//										.Select(availableLanguage => new DisplayLanguage(availableLanguage.Value,
//																						 availableLanguage.Key,
//																						 string.Equals(selectedLanguageId,
//																									   availableLanguage.Key,
//																									   StringComparison.Ordinal))));
//		}

//		private string _GenerateRegistrationKey()
//		{
//			Random random = new Random((int)(DateTime.Now.TimeOfDay.TotalMilliseconds));
//			StringBuilder registrationKey = new StringBuilder(MvcApplication.EdesiaSettings.Registration.RegistrationKeyLength);
//			Func<int, int> _Fold = (number =>
//				{
//					while (number / 10 > 0)
//					{
//						int sum = 0;
//						do
//						{
//							sum += number % 10;
//							number /= 10;
//						} while (number > 0);
//						number = sum;
//					}
//					return number;
//				});

//			for (int registrationKeyCharacterCount = 0; registrationKeyCharacterCount < registrationKey.Capacity; registrationKeyCharacterCount++)
//				registrationKey.Append(_Fold(random.Next(10000)));

//			return registrationKey.ToString();
//		}

//		private IApplicationUserStore _userStore = StoreFactory.ApplicationUserStore;
//	}
//}