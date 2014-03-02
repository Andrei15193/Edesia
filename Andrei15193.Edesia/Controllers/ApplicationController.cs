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

		//private string _Encode(string value)
		//{
		//	return Server.UrlEncode(value);
		//	//StringBuilder convertedValueStringBuilder = new StringBuilder();

		//	//if (value.Length > 0)
		//	//{
		//	//	convertedValueStringBuilder.Append(char.ConvertToUtf32(value, 0));
		//	//	for (int valueIndex = 1; valueIndex < value.Length; valueIndex++)
		//	//	{
		//	//		convertedValueStringBuilder.Append('-');
		//	//		convertedValueStringBuilder.Append(char.ConvertToUtf32(value, valueIndex));
		//	//	}
		//	//}

		//	//return convertedValueStringBuilder.ToString();
		//}
		//private string _Decode(string value)
		//{
		//	return Server.UrlDecode(value);
		//	//return string.Join(string.Empty, value.Split('-').Select(code =>
		//	//	{
		//	//		int codeValue;
		//	//		if (int.TryParse(code, out codeValue))
		//	//			return char.ConvertFromUtf32(codeValue);
		//	//		return char.ConvertFromUtf32(0);
		//	//	}));
		//}

		private const string _languageCookieName = "DisplayLanguage";
	}
}