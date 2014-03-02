using System.Configuration;
namespace Andrei15193.Edesia.Configuration
{
	public abstract class CloudConfigurableElement
		: ConfigurationElement
	{
		[ConfigurationProperty("UseCloudSettings", IsRequired = true, DefaultValue = true)]
		public bool UseCloudSettings
		{
			get
			{
				return (bool)this["UseCloudSettings"];
			}
			set
			{
				this["UseCloudSettings"] = value;
			}
		}
	}
}