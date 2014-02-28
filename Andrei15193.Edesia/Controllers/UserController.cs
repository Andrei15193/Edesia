using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Andrei15193.Edesia.Attributes;
using Andrei15193.Edesia.DataAccess;
using Andrei15193.Edesia.DataAccess.Xml;
using Andrei15193.Edesia.DataAccess.Xml.Azure;
using Andrei15193.Edesia.Extensions;
using Andrei15193.Edesia.Models;
using Andrei15193.Edesia.ViewModels;
using Microsoft.WindowsAzure;
namespace Andrei15193.Edesia.Controllers
{
	public class UserController
		: Controller
	{
		[ConfirmAccess(UserRoles.Client)]
		public ActionResult UserProfile()
		{
			return View();
		}
		[HttpGet]
		public ActionResult Register(string email, string key)
		{
			if (email == null || key == null)
				return View();
			else
				if (_userStore.ClearRegistrationKey(email, key))
					return View("_Notice", (object)("Contul dumneavoastră a fost creat cu success! Vă mulțumim ca ați ales să folosiți serviciile noastre!"));
				else
					return View("_Notice", (object)("Ne pare rău dar codul de înregistrare este prea vechi pentru a mai putea fi folosit. Trebuie sa vă reînregistrați."));
		}
		[HttpPost]
		public ActionResult Register(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid)
			{
				string registrationKey = _GenerateRegistrationKey();
				_userStore.AddUser(new User(registerViewModel.EMail, DateTime.Now)
					{
						Roles =
						{
							UserRoles.Client
						}
					}, registerViewModel.Password, registrationKey);

				new SmtpClient
				{
					Host = CloudConfigurationManager.GetSetting("smtp_host"),
					Port = int.Parse(CloudConfigurationManager.GetSetting("smtp_port")),
					EnableSsl = true,
					Credentials = new NetworkCredential(CloudConfigurationManager.GetSetting("smtp_username"), CloudConfigurationManager.GetSetting("smtp_password"))
				}.Send(new MailMessage
					{
						Subject = "Edesia Register",
						From = new MailAddress("andrei_fangli@hotmail.com", "Andrei Fangli"),
						To =
						{
							new MailAddress(registerViewModel.EMail)
						},
						DeliveryNotificationOptions = DeliveryNotificationOptions.Never,
						IsBodyHtml = true,
						BodyEncoding = Encoding.UTF8,
						Body = string.Format(_emailConfirmationMailBody,
											 new StringBuilder(Request.Url.Scheme).Append(Uri.SchemeDelimiter)
																				  .Append(Request.Url.Host)
																				  .Append(Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port)
																				  .Append("/User/Register")
																				  .ToString(),
											 Uri.EscapeDataString(registerViewModel.EMail),
											 Uri.EscapeDataString(registrationKey))
					});
				return View("_Notice", (object)("Am trimis un e-mail la adresa oferită cu detalii despre următorii pași care trebuie urmați pentru a vă înregistra."));
			}
			return View();
		}
		[HttpGet, ConfirmAccess]
		public ActionResult Logout()
		{
			_userStore.ClearAuthenticationKey(HttpContext.GetUser());
			FormsAuthentication.SignOut();
			Session.Abandon();
			return Redirect("/");
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
				User user = _userStore.Find(loginViewModel.Email, loginViewModel.Password);

				if (user != null)
				{
					FormsAuthentication.SignOut();
					HttpCookie authenticationCookie = FormsAuthentication.GetAuthCookie(loginViewModel.Email, true);

					_userStore.SetAuthenticationToken(user, authenticationCookie.Value, AuthenticationTokenType.Key);
					Response.SetCookie(authenticationCookie);
					HttpContext.SetUser(user, loginViewModel.Email);
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
		public ActionResult GetNavigationBar()
		{
			IList<NavigationBarAction> userActions = new List<NavigationBarAction>();

			if (User.Identity.IsAuthenticated)
				userActions.Add(new NavigationBarAction("Deconectare", "Logout", "User", Icons.User));
			else
			{
				userActions.Add(new NavigationBarAction("Conectare", "Login", "User", Icons.User));
				userActions.Add(new NavigationBarAction("Înregistrare", "Register", "User", Icons.New));
			}
			return View(userActions);
		}
		[NonAction]
		public void Login(string eMail, HttpContext context)
		{
			if (context.User.Identity.IsAuthenticated)
			{
				User user = _userStore.Find(eMail, context.Request.Cookies.Get(FormsAuthentication.FormsCookieName).Value, AuthenticationTokenType.Key);
				if (user != null)
					context.SetUser(user);
			}
		}

		private string _GenerateRegistrationKey()
		{
			Random random = new Random((int)(DateTime.Now.TimeOfDay.TotalMilliseconds));
			StringBuilder registrationKey = new StringBuilder(MvcApplication.EdesiaSettings.Registration.RegistrationKeyLength);
			Func<int, int> _Fold = (number =>
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
				registrationKey.Append(_Fold(random.Next(10000)));

			return registrationKey.ToString();
		}

		private IUserStore _userStore = new XmlUserStore(new AzureXmlDocumentProvider());
		private const string _emailConfirmationMailBody = @"<!doctype html>
<html>
<head>
	<meta charset=""utf-8"">
	<title>Edesia - Înregistrare</title>
	<style>
		body
		{{
			margin: 0;
			color: #000000;
			text-align: justify;
			font-family: 'Segoe UI Light_', 'Open Sans Light', Verdana, Arial, Helvetica, sans-serif;
			font-size: 10pt;
		}}

		h1
		{{
			margin: 0;
			padding: 10px;
			background-color: #008A00;
			color: #FFFFFF;
			font-size: 24pt;
			font-weight: 300;
		}}

		a
		{{
			color: #60A917;
			text-decoration: none;
		}}

			a:hover
			{{
				color: #7AD61D;
				text-decoration: none;
			}}

		.content
		{{
			padding-left: 15px;
			padding-right: 15px;
		}}

		p#signature
		{{
			margin-top: 30px;
			padding-top: 15px;
			height: 65px;
			background-color: #008A00;
			color: #FFFFFF;
		}}
	</style>
</head>
<body>
	<h1>Edesia - Înregistrare</h1>
	<div class=""content"">
		<p>
			Bună ziua! Pentru a putea finaliza înregistrarea trebuie să confirmați adresa de e-mail.
			Aceasta se face destul de simplu, trebuie doar să faceți un click pe linkul de
			mai jos. În caz că aveți probleme nu ezitați să ne contactați la adresa de e-mail:
			<small>edesia@outlook.com</small>. Vă mulțumim!
		</p>
		<p>
			<a href=""{0}?email={1}&key={2}"">{0}?email={1}&key={2}</a>
		</p>
	</div>
	<p id=""signature"" class=""content"">
		Vă dorim o zi superbă!
	</p>
</body>
</html>";
	}
}