using System.Configuration;
namespace Andrei15193.Edesia.Configuration
{
	public class RegistrationConfigurationElement
		: ConfigurationElement
	{
		[ConfigurationProperty("RegistrationKeyLength", DefaultValue = 7, IsRequired = true), IntegerValidator(MinValue = 0, MaxValue = 21, ExcludeRange = false)]
		public int RegistrationKeyLength
		{
			get
			{
				return (int)this["RegistrationKeyLength"];
			}
			set
			{
				this["RegistrationKeyLength"] = value;
			}
		}
		[ConfigurationProperty("RegistrationKeyHoursTimeout", DefaultValue = 24, IsRequired = true), IntegerValidator(MinValue = 5, MaxValue = 49, ExcludeRange = false)]
		public int RegistrationKeyHoursTimeout
		{
			get
			{
				return (int)this["RegistrationKeyHoursTimeout"];
			}
			set
			{
				this["RegistrationKeyHoursTimeout"] = value;
			}
		}
	}
}