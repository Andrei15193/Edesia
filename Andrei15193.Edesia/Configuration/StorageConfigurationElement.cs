using System.Configuration;
namespace Andrei15193.Edesia.Configuration
{
	public class StorageConfigurationElement
		: ConfigurationElement
	{
		[ConfigurationProperty("MembershipFileName", IsRequired = true)]
		public string MembershipFileName
		{
			get
			{
				return (string)this["MembershipFileName"];
			}
			set
			{
				this["MembershipFileName"] = value;
			}
		}
	}
}