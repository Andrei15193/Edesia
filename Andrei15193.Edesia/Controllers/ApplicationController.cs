using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Andrei15193.Edesia.ApplicationResources.Language;
namespace Andrei15193.Edesia.Controllers
{
	public abstract class ApplicationController
		: Controller
	{
		protected override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			base.OnResultExecuting(filterContext);
			LanguageResource.DisplayLanguageId = GetSelectedLanguageId();
		}
		protected HttpCookie GetLanguageCookie(string languageId)
		{
			return new HttpCookie(_languageCookieName, languageId ?? LanguageResource.DefaultLanguageId);
		}
		protected string GetSelectedLanguageId()
		{
			HttpCookie languageCookie = HttpContext.Request.Cookies[_languageCookieName];

			if (languageCookie != null)
				return languageCookie.Value;
			return LanguageResource.DefaultLanguageId;
		}

		private const string _languageCookieName = "DisplayLanguage";
	}
}