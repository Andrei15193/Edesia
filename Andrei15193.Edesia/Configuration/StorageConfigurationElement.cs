using System;
using System.Configuration;
using Microsoft.WindowsAzure;
namespace Andrei15193.Edesia.Configuration
{
	public class StorageConfigurationElement
		: CloudConfigurableElement
	{
		[ConfigurationProperty("StorageConnectionString")]
		public string StorageConnectionString
		{
			get
			{
				if (UseCloudSettings)
					return CloudConfigurationManager.GetSetting("StorageConnectionString");
				else
					return (string)this["StorageConnectionString"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["StorageConnectionString"] = value;
			}
		}
		[ConfigurationProperty("MembershipXmlDocumentFileName")]
		public string MembershipXmlDocumentFileName
		{
			get
			{
				if (UseCloudSettings)
					return CloudConfigurationManager.GetSetting("MembershipXmlDocumentFileName");
				else
					return (string)this["MembershipXmlDocumentFileName"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["MembershipXmlDocumentFileName"] = value;
			}
		}
		[ConfigurationProperty("XmlDocumentProviderType")]
		public string XmlDocumentProviderType
		{
			get
			{
				if (UseCloudSettings)
					return CloudConfigurationManager.GetSetting("XmlDocumentProviderType");
				else
					return (string)this["XmlDocumentProviderType"];
			}
			set
			{
				if (UseCloudSettings)
					throw new InvalidOperationException();
				this["XmlDocumentProviderType"] = value;
			}
		}
	}
}