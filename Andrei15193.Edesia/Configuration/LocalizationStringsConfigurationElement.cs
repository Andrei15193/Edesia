using System.Configuration;
namespace Andrei15193.Edesia.Configuration
{
	public class LocalizationStringsConfigurationElement
		: ConfigurationElement, ILocalizationConfigElement
	{
		#region ILocalizationConfigElement Members
		[ConfigurationProperty("LanguageDisplayName", IsRequired = true)]
		public string LanguageDisplayName
		{
			get
			{
				return (string)this["LanguageDisplayName"];
			}
			set
			{
				this["LanguageDisplayName"] = value;
			}
		}
		[ConfigurationProperty("LanguageId", IsRequired = true)]
		public string LanguageId
		{
			get
			{
				return (string)this["LanguageId"];
			}
			set
			{
				this["LanguageId"] = value;
			}
		}
		[ConfigurationProperty("ResourceFilesAssembly", IsRequired = true)]
		public string ResourceFilesAssembly
		{
			get
			{
				return (string)this["ResourceFilesAssembly"];
			}
			set
			{
				this["ResourceFilesAssembly"] = value;
			}
		}
		[ConfigurationProperty("EMailStringsResourceFile", IsRequired = true)]
		public string EMailStringsResourceFile
		{
			get
			{
				return (string)this["EMailStringsResourceFile"];
			}
			set
			{
				this["EMailStringsResourceFile"] = value;
			}
		}
		[ConfigurationProperty("ViewStringsResourceFile", IsRequired = true)]
		public string ViewStringsResourceFile
		{
			get
			{
				return (string)this["ViewStringsResourceFile"];
			}
			set
			{
				this["ViewStringsResourceFile"] = value;
			}
		}
		[ConfigurationProperty("ErrorStringsResourceFile", IsRequired = true)]
		public string ErrorStringsResourceFile
		{
			get
			{
				return (string)this["ErrorStringsResourceFile"];
			}
			set
			{
				this["ErrorStringsResourceFile"] = value;
			}
		}
		#endregion
	}
}