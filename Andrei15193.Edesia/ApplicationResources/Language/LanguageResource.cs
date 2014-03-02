using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
namespace Andrei15193.Edesia.ApplicationResources.Language
{
	public static class LanguageResource
	{
		static LanguageResource()
		{
			_languageResourceManagers = new SortedDictionary<string, ResourceManager>();
			foreach (ResourceManager resourceManager in from type in typeof(LanguageResource).Assembly.GetTypes()
														where (type != typeof(LanguageResource)
															   && string.Equals("Andrei15193.Edesia.ApplicationResources.Language", type.Namespace, StringComparison.Ordinal))
														let resourceManagerPropertyInfo = type.GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty, Type.DefaultBinder, typeof(ResourceManager), Type.EmptyTypes, null)
														where resourceManagerPropertyInfo != null
														select (ResourceManager)resourceManagerPropertyInfo.GetValue(null))
				_languageResourceManagers.Add(resourceManager.GetString("LanguageId"), resourceManager);
		}

		public static IEnumerable<KeyValuePair<string, string>> AvailableLanguages
		{
			get
			{
				return _languageResourceManagers.Select(languageResourceManager => new KeyValuePair<string, string>(languageResourceManager.Key, languageResourceManager.Value.GetString("LanguageDisplayName")));
			}
		}
		public static string DefaultLanguageId
		{
			get
			{
				return Romanian.LanguageId;
			}
		}
		public static string DisplayLanguageId
		{
			get
			{
				if (_displayLanguageId != null && _languageResourceManagers.ContainsKey(_displayLanguageId))
					return _displayLanguageId;
				return DefaultLanguageId;
			}
			set
			{
				_displayLanguageId = value;
			}
		}
		public static string EMailCopyPrompt
		{
			get
			{
				return ResourceManager.GetString("EMailCopyPrompt");
			}
		}
		public static string EMailDisplayName
		{
			get
			{
				return ResourceManager.GetString("EMailDisplayName");
			}
		}
		public static string EMailPrompt
		{
			get
			{
				return ResourceManager.GetString("EMailPrompt");
			}
		}
		public static string LanguageDisplayName
		{
			get
			{
				return ResourceManager.GetString("LanguageDisplayName");
			}
		}
		public static string LanguageLabel
		{
			get
			{
				return ResourceManager.GetString("LanguageLabel");
			}
		}
		public static string LoginLabel
		{
			get
			{
				return ResourceManager.GetString("LoginLabel");
			}
		}
		public static string LogoutLabel
		{
			get
			{
				return ResourceManager.GetString("LogoutLabel");
			}
		}
		public static string NotReadyYetMessage
		{
			get
			{
				return ResourceManager.GetString("NotReadyYetMessage");
			}
		}
		public static string PasswordCopyPrompt
		{
			get
			{
				return ResourceManager.GetString("PasswordCopyPrompt");
			}
		}
		public static string PasswordDisplayName
		{
			get
			{
				return ResourceManager.GetString("PasswordDisplayName");
			}
		}
		public static string PasswordPrompt
		{
			get
			{
				return ResourceManager.GetString("PasswordPrompt");
			}
		}
		public static string RegisterLabel
		{
			get
			{
				return ResourceManager.GetString("RegisterLabel");
			}
		}
		public static string NotReadyYetTitle
		{
			get
			{
				return ResourceManager.GetString("NotReadyYetTitle");
			}
		}
		public static string HomePageTitle
		{
			get
			{
				return ResourceManager.GetString("HomePageTitle");
			}
		}
		public static string LanguageId
		{
			get
			{
				return ResourceManager.GetString("LanguageId");
			}
		}
		public static ResourceManager ResourceManager
		{
			get
			{
				ResourceManager languageResourceManager;
				if (_languageResourceManagers.TryGetValue(_displayLanguageId, out languageResourceManager))
					return languageResourceManager;
				return _languageResourceManagers[Romanian.LanguageId];
			}
		}
		
		[ThreadStatic]
		private static string _displayLanguageId;
		private static readonly IDictionary<string, ResourceManager> _languageResourceManagers;
	}
}