using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Andrei15193.DependencyInjection;
using Andrei15193.DependencyInjection.Configuration;
namespace Andrei15193.Edesia.Controllers
{
	public abstract class ApplicationController
		: Controller
	{
		static ApplicationController()
		{
			_dependencyContainer = new DependencyContainer((DependencyInjectionConfigurationSection)WebConfigurationManager.GetSection("DependencyInjection"));
		}

		protected override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			base.OnResultExecuting(filterContext);
			Resources.Strings.SelectedLangaugeId = GetSelectedLanguageId();
		}
		protected HttpCookie GetLanguageCookie(string languageId)
		{
			return new HttpCookie(_languageCookieName, languageId ?? Resources.Strings.DefaultLanguageId);
		}
		protected string GetSelectedLanguageId()
		{
			HttpCookie languageCookie = HttpContext.Request.Cookies[_languageCookieName];

			if (languageCookie != null)
				return languageCookie.Value;
			return Resources.Strings.DefaultLanguageId;
		}
		protected static DependencyContainer DependencyContainer
		{
			get
			{
				return _dependencyContainer;
			}
		}

		private const string _languageCookieName = "DisplayLanguage";
		private static readonly DependencyContainer _dependencyContainer;
	}
}