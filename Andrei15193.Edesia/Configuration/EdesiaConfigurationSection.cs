﻿using System.Configuration;
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
	}
}