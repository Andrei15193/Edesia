using System;
namespace Andrei15193.Edesia.Models
{
	public struct DisplayLanguage
	{
		public DisplayLanguage(string languageDisplayName, string languageId, bool isSelected = false)
		{
			if (languageDisplayName == null)
				throw new ArgumentNullException("languageDisplayName");
			if (string.IsNullOrEmpty(languageDisplayName) || string.IsNullOrWhiteSpace(languageDisplayName))
				throw new ArgumentException("Cannot be empty or whitespace!", "languageDisplayName");
			if (languageId == null)
				throw new ArgumentNullException("languageId");
			if (string.IsNullOrEmpty(languageId) || string.IsNullOrWhiteSpace(languageId))
				throw new ArgumentException("Cannot be empty or whitespace!", "languageId");

			_languageDisplayName = languageDisplayName;
			_languageId = languageId;
			_isSelected = isSelected;
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
		}
		public string LanguageDisplayName
		{
			get
			{
				return _languageDisplayName;
			}
		}
		public string LanguageId
		{
			get
			{
				return _languageId;
			}
		}

		private readonly bool _isSelected;
		private readonly string _languageDisplayName;
		private readonly string _languageId;
	}
}