using System;
using System.Configuration;
using Microsoft.WindowsAzure;
namespace Andrei15193.Edesia.Configuration
{
	public class EmailConfigurationElement
		: CloudConfigurableElement
	{
		[ConfigurationProperty("SmtpHost")]
		public string SmtpHost
		{
			get
			{
				if (UseCloudSettings)
					return CloudConfigurationManager.GetSetting("SmtpHost");
				else
					return (string)this["SmtpHost"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["SmtpHost"] = value;
			}
		}
		[ConfigurationProperty("SmtpPort", DefaultValue = 25)]
		public int SmtpPort
		{
			get
			{
				if (UseCloudSettings)
					return int.Parse(CloudConfigurationManager.GetSetting("SmtpPort"));
				else
					return (int)this["SmtpPort"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["SmtpPort"] = value;
			}
		}
		[ConfigurationProperty("SmtpUsername")]
		public string SmtpUsername
		{
			get
			{
				if (UseCloudSettings)
					return CloudConfigurationManager.GetSetting("SmtpUsername");
				else
					return (string)this["SmtpUsername"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["SmtpUsername"] = value;
			}
		}
		[ConfigurationProperty("SmtpPassword")]
		public string SmtpPassword
		{
			get
			{
				if (UseCloudSettings)
					return CloudConfigurationManager.GetSetting("SmtpPassword");
				else
					return (string)this["SmtpPassword"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["SmtpPassword"] = value;
			}
		}
		[ConfigurationProperty("SenderEMailAddress")]
		public string SenderEMailAddress
		{
			get
			{
				if (UseCloudSettings)
					return CloudConfigurationManager.GetSetting("SenderEMailAddress");
				else
					return (string)this["SenderEMailAddress"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["SenderEMailAddress"] = value;
			}
		}
		[ConfigurationProperty("SenderDisplayName")]
		public string SenderDisplayName
		{
			get
			{
				if (UseCloudSettings)
					return CloudConfigurationManager.GetSetting("SenderDisplayName");
				else
					return (string)this["SenderDisplayName"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["SenderDisplayName"] = value;
			}
		}
	}
}