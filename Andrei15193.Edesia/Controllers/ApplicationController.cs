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
			HttpCookie languageCookie = filterContext.HttpContext.Request.Cookies[_languageCookieName];

			if (languageCookie != null)
				LanguageResource.DisplayLanguage = _Decode(languageCookie.Value);
		}
		protected HttpCookie GetLanguageCookie(string language)
		{
			return new HttpCookie(_languageCookieName, _Encode(language ?? LanguageResource.DefaultLanguage));
		}
		protected string GetSelectedLanguage()
		{
			HttpCookie languageCookie = HttpContext.Request.Cookies[_languageCookieName];

			if (languageCookie != null)
				return _Decode(languageCookie.Value);
			return LanguageResource.DefaultLanguage;
		}

		private string _Encode(string value)
		{
			StringBuilder convertedValueStringBuilder = new StringBuilder();

			if (value.Length > 0)
			{
				convertedValueStringBuilder.Append(char.ConvertToUtf32(value, 0));
				for (int valueIndex = 1; valueIndex < value.Length; valueIndex++)
				{
					convertedValueStringBuilder.Append('-');
					convertedValueStringBuilder.Append(char.ConvertToUtf32(value, valueIndex));
				}
			}

			return convertedValueStringBuilder.ToString();
		}
		private string _Decode(string value)
		{
			return string.Join(string.Empty, value.Split('-').Select(code =>
				{
					int codeValue;
					if (int.TryParse(code, out codeValue))
						return char.ConvertFromUtf32(codeValue);
					return char.ConvertFromUtf32(0);
				}));
		}

		private const string _languageCookieName = "DisplayLanguage";
	}
}