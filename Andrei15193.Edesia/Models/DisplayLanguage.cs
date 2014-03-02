using System;
namespace Andrei15193.Edesia.Models
{
	public struct DisplayLanguage
	{
		public DisplayLanguage(string languageName, bool isSelected = false)
		{
			if (languageName == null)
				throw new ArgumentNullException("language");
			if (string.IsNullOrEmpty(languageName) || string.IsNullOrWhiteSpace(languageName))
				throw new ArgumentException("Cannot be empty or whitespace!", "language");

			_languageName = languageName;
			_isSelected = isSelected;
		}

		public string LanguageName
		{
			get
			{
				return _languageName;
			}
		}
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
		}

		private readonly bool _isSelected;
		private readonly string _languageName;
	}
}