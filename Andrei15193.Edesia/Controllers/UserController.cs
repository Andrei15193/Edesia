using System.Linq;
using System.Web.Mvc;
using Andrei15193.Edesia.Controllers.Validation;
using Andrei15193.Edesia.Data;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Controllers
{
	public class UserController
		: Controller
	{
		public ActionResult UserProfile()
		{
			return View(Session["user"]);
		}
		public ActionResult Login()
		{
			if (Request.Form.HasKeys()
				&& Request.Form.AllKeys.Contains("username")
				&& Request.Form.AllKeys.Contains("password"))
			{
				string username = Request.Form["username"];
				string password = Request.Form["password"];

				User authenticatedUser = MockData.Users.FirstOrDefault(user => (user.Username == username && user.Password == password));
				if (authenticatedUser != null)
				{
					Session.Add("user", authenticatedUser);
					return Redirect("/");
				}
				else
					ViewData.Add("validationError", new ValidationResult("Numele de utilizator sau parola sunt invalide."));
			}
			return View();
		}
		public ActionResult Logout()
		{
			Session.RemoveAll();
			return Redirect("/");
		}
	}
}