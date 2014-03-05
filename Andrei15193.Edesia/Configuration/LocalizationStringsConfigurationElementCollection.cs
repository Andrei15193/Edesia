using System.Configuration;
namespace Andrei15193.Edesia.Configuration
{
	[ConfigurationCollection(typeof(LocalizationStringsConfigurationElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class LocalizationStringsConfigurationElementCollection
		: ConfigurationElementCollection
	{
		[ConfigurationProperty("DefaultLanguageId", IsRequired = true)]
		public string DefaultLanguageId
		{
			get
			{
				return (string)this["DefaultLanguageId"];
			}
			set
			{
				this["DefaultLanguageId"] = value;
			}
		}
		protected override ConfigurationElement CreateNewElement()
		{
			return new LocalizationStringsConfigurationElement();
		}
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((LocalizationStringsConfigurationElement)element).LanguageId;
		}
	}
}