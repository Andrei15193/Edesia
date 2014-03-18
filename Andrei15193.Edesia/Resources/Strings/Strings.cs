using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using Andrei15193.Edesia.Settings;
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
						_registeredLanguageSettingsLock.EnterReadLock();
						return _defaultLanguageId;
					}
					finally
					{
						_registeredLanguageSettingsLock.ExitReadLock();
					}
				}
				set
				{
					try
					{
						_registeredLanguageSettingsLock.EnterWriteLock();
						_defaultLanguageId = value;
					}
					finally
					{
						_registeredLanguageSettingsLock.ExitWriteLock();
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
						Tuple<ILanguageSettings, ResourceManager, ResourceManager, ResourceManager> selectedLanguage;
						_registeredLanguageSettingsLock.EnterReadLock();

						if (!_registeredLanguageSettings.TryGetValue(value, out selectedLanguage))
							selectedLanguage = _registeredLanguageSettings[DefaultLanguageId];

						_selectedLanguageSettings = selectedLanguage;
					}
					finally
					{
						_registeredLanguageSettingsLock.ExitReadLock();
					}
				}
			}
			public static ILanguageSettings SelectedLanguage
			{
				get
				{
					if (_selectedLanguageSettings == null)
						SelectedLangaugeId = DefaultLanguageId;
					return _selectedLanguageSettings.Item1;
				}
			}
			public static IReadOnlyList<ILanguageSettings> LanguageSpecifications
			{
				get
				{
					try
					{
						_registeredLanguageSettingsLock.EnterReadLock();
						return _registeredLanguageSettings.Values
														  .Select(languageSettings => languageSettings.Item1)
														  .OrderBy(languageSpecification => languageSpecification.LanguageDisplayName)
														  .ToList();
					}
					finally
					{
						_registeredLanguageSettingsLock.ExitReadLock();
					}
				}
			}
			internal static ResourceManager EMailStringsResourceManager
			{
				get
				{
					return _selectedLanguageSettings.Item2;
				}
			}
			internal static ResourceManager ErrorStringsResourceManager
			{
				get
				{
					return _selectedLanguageSettings.Item3;
				}
			}
			internal static ResourceManager ViewStringsResourceManager
			{
				get
				{
					return _selectedLanguageSettings.Item4;
				}
			}

			internal static void RegisterLanguageStrings(ILanguageSettings languageSetting)
			{
				if (languageSetting == null)
					throw new ArgumentNullException("languageSetting");

				Assembly resourceAssembly = Assembly.Load(new AssemblyName(languageSetting.ResourceFilesAssemblyName));
				if (resourceAssembly == null)
					throw new ArgumentException(string.Format("Assembly {0} is not loaded!", languageSetting.ResourceFilesAssemblyName), "localizationConfigElement");

				try
				{
					_registeredLanguageSettingsLock.EnterWriteLock();
					_registeredLanguageSettings.Add(languageSetting.LanguageId,
													Tuple.Create(languageSetting,
																 new ResourceManager(languageSetting.EMailStringsResourceFile, resourceAssembly),
																 new ResourceManager(languageSetting.ErrorStringsResourceFile, resourceAssembly),
																 new ResourceManager(languageSetting.ViewStringsResourceFile, resourceAssembly)));
				}
				finally
				{
					_registeredLanguageSettingsLock.ExitWriteLock();
				}
			}

			private static string _defaultLanguageId;
			[ThreadStatic]
			private static Tuple<ILanguageSettings, ResourceManager, ResourceManager, ResourceManager> _selectedLanguageSettings;
			private static readonly ReaderWriterLockSlim _registeredLanguageSettingsLock = new ReaderWriterLockSlim();
			private static readonly IDictionary<string, Tuple<ILanguageSettings, ResourceManager, ResourceManager, ResourceManager>> _registeredLanguageSettings = new SortedDictionary<string, Tuple<ILanguageSettings, ResourceManager, ResourceManager, ResourceManager>>();
		}
	}
}