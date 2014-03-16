using System;
namespace Andrei15193.Edesia.Settings
{
	public sealed class LanguageSettings
		: ILanguageSettings
	{
		public LanguageSettings(string languageDisplayName, string languageId, string resourceFilesAssemblyName, string eMailStringsResourceFile, string errorStringsResourceFile, string viewStringsResourceFile)
		{
			if (languageDisplayName == null)
				throw new ArgumentNullException("languageDisplayName");
			if (string.IsNullOrEmpty(languageDisplayName) || string.IsNullOrWhiteSpace(languageDisplayName))
				throw new ArgumentException("cannot be empty or whitespace!", "languageDisplayName");
			if (languageId == null)
				throw new ArgumentNullException("languageId");
			if (string.IsNullOrEmpty(languageId) || string.IsNullOrWhiteSpace(languageId))
				throw new ArgumentException("cannot be empty or whitespace!", "languageId");

			if (resourceFilesAssemblyName == null)
				throw new ArgumentNullException("resourceFilesAssemblyName");
			if (string.IsNullOrEmpty(resourceFilesAssemblyName) || string.IsNullOrWhiteSpace(resourceFilesAssemblyName))
				throw new ArgumentException("cannot be empty or whitespace!", "resourceFilesAssemblyName");

			if (eMailStringsResourceFile == null)
				throw new ArgumentNullException("eMailStringsResourceFile");
			if (string.IsNullOrEmpty(eMailStringsResourceFile) || string.IsNullOrWhiteSpace(eMailStringsResourceFile))
				throw new ArgumentException("cannot be empty or whitespace!", "eMailStringsResourceFile");
			if (errorStringsResourceFile == null)
				throw new ArgumentNullException("errorStringsResourceFile");
			if (string.IsNullOrEmpty(errorStringsResourceFile) || string.IsNullOrWhiteSpace(errorStringsResourceFile))
				throw new ArgumentException("cannot be empty or whitespace!", "errorStringsResourceFile");
			if (viewStringsResourceFile == null)
				throw new ArgumentNullException("viewStringsResourceFile");
			if (string.IsNullOrEmpty(viewStringsResourceFile) || string.IsNullOrWhiteSpace(viewStringsResourceFile))
				throw new ArgumentException("cannot be empty or whitespace!", "viewStringsResourceFile");

			_languageDisplayName = languageDisplayName;
			_languageId = languageId;
			_resourceFilesAssemblyName = resourceFilesAssemblyName;
			_eMailStringsResourceFile = eMailStringsResourceFile;
			_errorStringsResourceFile = errorStringsResourceFile;
			_viewStringsResourceFile = viewStringsResourceFile;
		}

		#region ILanguageSetting Members
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
		public string ResourceFilesAssemblyName
		{
			get
			{
				return _resourceFilesAssemblyName;
			}
		}
		public string EMailStringsResourceFile
		{
			get
			{
				return _eMailStringsResourceFile;
			}
		}
		public string ErrorStringsResourceFile
		{
			get
			{
				return _errorStringsResourceFile;
			}
		}
		public string ViewStringsResourceFile
		{
			get
			{
				return _viewStringsResourceFile;
			}
		}
		#endregion

		private readonly string _languageDisplayName;
		private readonly string _languageId;
		private readonly string _resourceFilesAssemblyName;
		private readonly string _eMailStringsResourceFile;
		private readonly string _errorStringsResourceFile;
		private readonly string _viewStringsResourceFile;
	}
}