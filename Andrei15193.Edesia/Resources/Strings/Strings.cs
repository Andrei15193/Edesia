using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
namespace Andrei15193.Edesia
{
	public static partial class Resources
	{
		public static partial class Strings
		{
			public static string DefaultLanguageId
			{
				get
				{
					try
					{
						_localizedLanguageSpecificationsLock.EnterReadLock();
						return _defaultLanguageId;
					}
					finally
					{
						_localizedLanguageSpecificationsLock.ExitReadLock();
					}
				}
				set
				{
					try
					{
						_localizedLanguageSpecificationsLock.EnterWriteLock();
						_defaultLanguageId = value;
					}
					finally
					{
						_localizedLanguageSpecificationsLock.ExitWriteLock();
					}
				}
			}
			public static string SelectedLangaugeId
			{
				get
				{
					return SelectedLanguage.LanguageId;
				}
				set
				{
					try
					{
						ILocalizedLanguageSpecification selectedLanguage;
						_localizedLanguageSpecificationsLock.EnterReadLock();

						if (!_localizedLanguageSpecifications.TryGetValue(value, out selectedLanguage))
							selectedLanguage = _localizedLanguageSpecifications[DefaultLanguageId];

						_selectedLanguage = selectedLanguage;
					}
					finally
					{
						_localizedLanguageSpecificationsLock.ExitReadLock();
					}
				}
			}
			public static ILocalizedLanguageSpecification SelectedLanguage
			{
				get
				{
					return _selectedLanguage;
				}
			}
			public static IReadOnlyList<ILanguageSpecification> LanguageSpecifications
			{
				get
				{
					try
					{
						_localizedLanguageSpecificationsLock.EnterReadLock();
						return _localizedLanguageSpecifications.Values
															   .OrderBy(languageSpecification => languageSpecification.LanguageDisplayName)
															   .ToList();
					}
					finally
					{
						_localizedLanguageSpecificationsLock.ExitReadLock();
					}
				}
			}
			public static void RegisterLanguageStrings(ILocalizationConfigElement localizationConfigElement)
			{
				if (localizationConfigElement == null)
					throw new ArgumentNullException("localizationConfigElement");

				Assembly resourceAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => string.Equals(assembly.GetName().Name, localizationConfigElement.ResourceFilesAssembly, StringComparison.Ordinal));
				if (resourceAssembly == null)
					throw new ArgumentException(string.Format("Assembly {0} is not loaded!", localizationConfigElement.ResourceFilesAssembly), "localizationConfigElement");

				try
				{
					_localizedLanguageSpecificationsLock.EnterWriteLock();
					_localizedLanguageSpecifications.Add(localizationConfigElement.LanguageId,
														 new LocalizedLanguageSpecification(localizationConfigElement.LanguageDisplayName,
																							localizationConfigElement.LanguageId,
																							new ResourceManager(localizationConfigElement.EMailStringsResourceFile, resourceAssembly),
																							new ResourceManager(localizationConfigElement.ErrorStringsResourceFile, resourceAssembly),
																							new ResourceManager(localizationConfigElement.ViewStringsResourceFile, resourceAssembly)));
				}
				finally
				{
					_localizedLanguageSpecificationsLock.ExitWriteLock();
				}
			}

			private class LanguageSpecification
				: ILanguageSpecification
			{
				public LanguageSpecification(string languageDisplayName, string languageId)
				{
					if (languageDisplayName == null)
						throw new ArgumentNullException("languageDisplayName");
					if (string.IsNullOrEmpty(languageDisplayName) || string.IsNullOrWhiteSpace(languageDisplayName))
						throw new ArgumentException("Cannot be empty or whitespace!", "languageDisplayName");
					if (languageId == null)
						throw new ArgumentNullException("languageId");
					if (string.IsNullOrEmpty(languageId) || string.IsNullOrWhiteSpace(languageId))
						throw new ArgumentException("Cannot be empty or whitespace!", "languageId");

					_languageDisplayName = languageDisplayName.Trim();
					_languageId = languageId.Trim();
				}

				#region ILanguageSpecification Members
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
				#endregion

				private readonly string _languageDisplayName;
				private readonly string _languageId;
			}
			private class LocalizedLanguageSpecification
				: LanguageSpecification, ILocalizedLanguageSpecification
			{
				public LocalizedLanguageSpecification(string languageDisplayName, string languageId, ResourceManager eMailStringsResourceManager, ResourceManager errorStringsResourceManager, ResourceManager viewStringsResourceManager)
					: base(languageDisplayName, languageId)
				{
					if (eMailStringsResourceManager == null)
						throw new ArgumentNullException("eMailStringsResourceManager");
					if (errorStringsResourceManager == null)
						throw new ArgumentNullException("errorStringsResourceManager");
					if (viewStringsResourceManager == null)
						throw new ArgumentNullException("viewStringsResourceManager");

					_eMailStringsResourceManager = eMailStringsResourceManager;
					_errorStringsResourceManager = errorStringsResourceManager;
					_viewStringsResourceManager = viewStringsResourceManager;
				}

				#region ILocalizedLanguageSpecification Members
				public ResourceManager EMailStringsResourceManager
				{
					get
					{
						return _eMailStringsResourceManager;
					}
				}
				public ResourceManager ErrorStringsResourceManager
				{
					get
					{
						return _errorStringsResourceManager;
					}
				}
				public ResourceManager ViewStringsResourceManager
				{
					get
					{
						return _viewStringsResourceManager;
					}
				}
				#endregion

				private ResourceManager _eMailStringsResourceManager;
				private ResourceManager _errorStringsResourceManager;
				private ResourceManager _viewStringsResourceManager;
			}

			private static string _defaultLanguageId;
			[ThreadStatic]
			private static ILocalizedLanguageSpecification _selectedLanguage;
			private static readonly ReaderWriterLockSlim _localizedLanguageSpecificationsLock = new ReaderWriterLockSlim();
			private static readonly IDictionary<string, ILocalizedLanguageSpecification> _localizedLanguageSpecifications = new SortedDictionary<string, ILocalizedLanguageSpecification>();
		}
	}
}