using System.Collections.Generic;
namespace Andrei15193.Edesia.Settings
{
	public sealed class LocalizationSettings
		: ILocalizationSettings
	{
		#region ILocalizationSettings Members
		public IList<ILanguageSettings> LanguageSettings
		{
			get
			{
				return _langaugeSettings;
			}
		}
		#endregion

		private readonly IList<ILanguageSettings> _langaugeSettings = new List<ILanguageSettings>();
	}
}