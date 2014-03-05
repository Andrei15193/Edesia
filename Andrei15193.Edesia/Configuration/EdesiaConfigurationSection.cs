using System.Collections.Generic;
using System.Configuration;
namespace Andrei15193.Edesia.Configuration
{
	public class EdesiaConfigurationSection
		: ConfigurationSection
	{
		[ConfigurationProperty("RegistrationSettings", IsRequired = true)]
		public RegistrationConfigurationElement Registration
		{
			get
			{
				return (RegistrationConfigurationElement)this["RegistrationSettings"];
			}
			set
			{
				this["RegistrationSettings"] = value;
			}
		}
		[ConfigurationProperty("StorageSettings", IsRequired = true)]
		public StorageConfigurationElement StorageSettings
		{
			get
			{
				return (StorageConfigurationElement)this["StorageSettings"];
			}
			set
			{
				this["StorageSettings"] = value;
			}
		}
		[ConfigurationProperty("EmailSettings", IsRequired = true)]
		public EmailConfigurationElement EmailSettings
		{
			get
			{
				return (EmailConfigurationElement)this["EmailSettings"];
			}
			set
			{
				this["EmailSettings"] = value;
			}
		}
		[ConfigurationProperty("LocalizationStrings", IsRequired = true)]
		public LocalizationStringsConfigurationElementCollection LocalizationStrings
		{
			get
			{
				return (LocalizationStringsConfigurationElementCollection)this["LocalizationStrings"];
			}
			set
			{
				this["LocalizationStrings"] = value;
			}
		}
	}
}