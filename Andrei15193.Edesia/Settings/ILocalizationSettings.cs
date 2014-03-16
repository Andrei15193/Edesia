using System.Collections.Generic;
namespace Andrei15193.Edesia.Settings
{
	public interface ILocalizationSettings
	{
		IList<ILanguageSettings> LanguageSettings
		{
			get;
		}
	}
}