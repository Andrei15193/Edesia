using System;
namespace Andrei15193.Edesia
{
	public static partial class Resources
	{
		public static partial class Strings
		{
			public static class EMail
			{
				public static string GetString(string key)
				{
					string value = EMailStringsResourceManager.GetString(key);

					if (value == null)
						throw new InvalidOperationException(string.Format("Key {0} does not exist in Resources.Strings.EMail[{1}]", key, SelectedLangaugeId));

					return value;
				}
				public static string RegistrationBodyFormat
				{
					get
					{
						return GetString("RegistrationBodyFormat");
					}
				}
			}
		}
	}
}