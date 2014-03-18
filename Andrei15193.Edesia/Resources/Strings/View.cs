using System;
namespace Andrei15193.Edesia
{
	public static partial class Resources
	{
		public static partial class Strings
		{
			public static class View
			{
				public static string GetString(string key)
				{
					string value = ViewStringsResourceManager.GetString(key);

					if (value == null)
						throw new InvalidOperationException(string.Format("Key {0} does not exist in Resources.Strings.View[{1}]", key, SelectedLangaugeId));

					return value;
				}
				public static string AcceptButtonText
				{
					get
					{
						return GetString("AcceptButtonText");
					}
				}
				public static string EMailAddressCopyPlaceholder
				{
					get
					{
						return GetString("EMailAddressCopyPlaceholder");
					}
				}
				public static string EMailAddressLabel
				{
					get
					{
						return GetString("EMailAddressLabel");
					}
				}
				public static string EMailAddressPlaceholder
				{
					get
					{
						return GetString("EMailAddressPlaceholder");
					}
				}
				public static string HomePageNoticeParagraph1
				{
					get
					{
						return GetString("HomePageNoticeParagraph1");
					}
				}
				public static string HomePageNoticeTitle
				{
					get
					{
						return GetString("HomePageNoticeTitle");
					}
				}
				public static string HomePageTitle
				{
					get
					{
						return GetString("HomePageTitle");
					}
				}
				public static string LanguageLabel
				{
					get
					{
						return GetString("LanguageLabel");
					}
				}
				public static string LoginLabel
				{
					get
					{
						return GetString("LoginLabel");
					}
				}
				public static string LogoutLabel
				{
					get
					{
						return GetString("LogoutLabel");
					}
				}
				public static string PasswordCopyPlaceholder
				{
					get
					{
						return GetString("PasswordCopyPlaceholder");
					}
				}
				public static string PasswordLabel
				{
					get
					{
						return GetString("PasswordLabel");
					}
				}
				public static string PasswordPlaceholder
				{
					get
					{
						return GetString("PasswordPlaceholder");
					}
				}
				public static string ProfilePageTitle
				{
					get
					{
						return GetString("ProfilePageTitle");
					}
				}
				public static string RegisterButtonText
				{
					get
					{
						return GetString("RegisterButtonText");
					}
				}
				public static string RegisterLabel
				{
					get
					{
						return GetString("RegisterLabel");
					}
				}
				public static string RegisterMailSendNoticeParagraph1
				{
					get
					{
						return GetString("RegisterMailSendNoticeParagraph1");
					}
				}
				public static string RegistrationCompleteNoticeParagraph1
				{
					get
					{
						return GetString("RegistrationCompleteNoticeParagraph1");
					}
				}
				public static string RolesLabel
				{
					get
					{
						return GetString("RolesLabel");
					}
				}
				public static string FirstNameLabel
				{
					get
					{
						return GetString("FirstNameLabel");
					}
				}
				public static string FirstNamePlaceholder
				{
					get
					{
						return GetString("FirstNamePlaceholder");
					}
				}
				public static string LastNameLabel
				{
					get
					{
						return GetString("LastNameLabel");
					}
				}
				public static string LastNamePlaceholder
				{
					get
					{
						return GetString("LastNamePlaceholder");
					}
				}
			}
		}
	}
}