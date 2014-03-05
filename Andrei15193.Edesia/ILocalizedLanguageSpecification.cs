using System.Resources;
namespace Andrei15193.Edesia
{
	public interface ILocalizedLanguageSpecification
		: ILanguageSpecification
	{
		ResourceManager EMailStringsResourceManager
		{
			get;
		}
		ResourceManager ErrorStringsResourceManager
		{
			get;
		}
		ResourceManager ViewStringsResourceManager
		{
			get;
		}
	}
}