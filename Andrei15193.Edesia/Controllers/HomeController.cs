using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Controllers
{
	public class HomeController
		: Controller
	{
		public ActionResult Default()
		{
			return View();
		}

		public ActionResult GetMenu()
		{
			User user = Session["user"] as User;
			if (user != null)
				return View(new[]
					{
						Tuple.Create("Acasă", "Default", "Home"),
						Tuple.Create("Profil", "UserProfile", "User"),
						Tuple.Create("Deconectare", "Logout", "User")
					});
			else
				return View(new[]
					{
						Tuple.Create("Acasă", "Default", "Home"),
						Tuple.Create("Conectare", "Login", "User")
					});
		}
	}
}