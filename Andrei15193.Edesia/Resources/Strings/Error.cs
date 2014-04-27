using System;
namespace Andrei15193.Edesia
{
	public static partial class Resources
	{
		public static partial class Strings
		{
			public static class Error
			{
				public static string GetString(string key)
				{
					string value = ErrorStringsResourceManager.GetString(key);

					if (value == null)
						throw new InvalidOperationException(string.Format("Key {0} does not exist in Resources.Strings.Error[{1}]", key, SelectedLangaugeId));

					return value;
				}
				public static string RegistrationTokenExpiredMessage
				{
					get
					{
						return GetString("RegistrationBodyFormat");
					}
				}
				public static string DuplicateEMailMessageFormat
				{
					get
					{
						return GetString("DuplicateEMailMessageFormat");
					}
				}
				public static string EMailAddressesAreNotEqualMessage
				{
					get
					{
						return GetString("EMailAddressesAreNotEqualMessage");
					}
				}
				public static string PasswordsAreNotEqualMessage
				{
					get
					{
						return GetString("PasswordsAreNotEqualMessage");
					}
				}
				public static string InvalidEMailAddressMessage
				{
					get
					{
						return GetString("InvalidEMailAddressMessage");
					}
				}
				public static string MissingEMailAddressMessage
				{
					get
					{
						return GetString("MissingEMailAddressMessage");
					}
				}
				public static string MissingPasswordMessage
				{
					get
					{
						return GetString("MissingPasswordMessage");
					}
				}
				public static string InvalidFirstNameMessage
				{
					get
					{
						return GetString("InvalidFirstNameMessage");
					}
				}
				public static string MissingFirstNameMessage
				{
					get
					{
						return GetString("MissingFirstNameMessage");
					}
				}
				public static string InvalidLastNameMessage
				{
					get
					{
						return GetString("InvalidLastNameMessage");
					}
				}
				public static string MissingLastNameMessage
				{
					get
					{
						return GetString("MissingLastNameMessage");
					}
				}
				public static string InvalidCredentialsMessage
				{
					get
					{
						return GetString("InvalidCredentialsMessage");
					}
				}
			}
		}
	}
}