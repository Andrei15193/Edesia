using System;
using System.Net;
using System.Net.Mail;
using Microsoft.WindowsAzure;
namespace Andrei15193.Edesia.Settings.Azure
{
	public class AzureEMailSettings
		: IEMailSettings
	{
		public AzureEMailSettings(string smtpHostCloudSettingName, string smtpPortCloudSettingName, string usernameCloudSettingName, string passwordCloudSettingName, string senderDisplayNameCloudSettingName, string senderEMailAddressCloudSettingName)
		{
			_ValidateNullEmptyOrWhiteSpace(smtpHostCloudSettingName, "smtpHostCloudSettingName");
			_ValidateNullEmptyOrWhiteSpace(smtpPortCloudSettingName, "smtpPortCloudSettingName");
			_ValidateNullEmptyOrWhiteSpace(usernameCloudSettingName, "usernameCloudSettingName");
			_ValidateNullEmptyOrWhiteSpace(passwordCloudSettingName, "passwordCloudSettingName");
			_ValidateNullEmptyOrWhiteSpace(senderDisplayNameCloudSettingName, "senderDisplayNameCloudSettingName");
			_ValidateNullEmptyOrWhiteSpace(senderEMailAddressCloudSettingName, "senderEMailAddressCloudSettingName");

			_smtpHostCloudSettingName = smtpHostCloudSettingName;
			_smtpPortCloudSettingName = smtpPortCloudSettingName;
			_usernameCloudSettingName = usernameCloudSettingName;
			_passwordCloudSettingName = passwordCloudSettingName;
			_senderDisplayNameCloudSettingName = senderDisplayNameCloudSettingName;
			_senderEMailAddressCloudSettingName = senderEMailAddressCloudSettingName;
		}

		#region IEmailSettings Members
		public string SmtpHost
		{
			get
			{
				return CloudConfigurationManager.GetSetting(_smtpHostCloudSettingName);
			}
		}
		public int SmtpPort
		{
			get
			{
				return int.Parse(CloudConfigurationManager.GetSetting(_smtpPortCloudSettingName));
			}
		}
		public NetworkCredential Credentials
		{
			get
			{
				return new NetworkCredential(CloudConfigurationManager.GetSetting(_usernameCloudSettingName), CloudConfigurationManager.GetSetting(_passwordCloudSettingName));
			}
		}
		public MailAddress SenderMailAddress
		{
			get
			{
				return new MailAddress(CloudConfigurationManager.GetSetting(_senderEMailAddressCloudSettingName), CloudConfigurationManager.GetSetting(_senderDisplayNameCloudSettingName));
			}
		}
		#endregion

		private static void _ValidateNullEmptyOrWhiteSpace(string parameterValue, string parameterName)
		{
			if (parameterValue == null)
				throw new ArgumentNullException(parameterName);
			if (string.IsNullOrEmpty(parameterValue) || string.IsNullOrWhiteSpace(parameterValue))
				throw new ArgumentException("Cannot be empty or whitespace!", parameterName);
		}
		private readonly string _smtpHostCloudSettingName;
		private readonly string _smtpPortCloudSettingName;
		private readonly string _usernameCloudSettingName;
		private readonly string _passwordCloudSettingName;
		private readonly string _senderDisplayNameCloudSettingName;
		private readonly string _senderEMailAddressCloudSettingName;
	}
}