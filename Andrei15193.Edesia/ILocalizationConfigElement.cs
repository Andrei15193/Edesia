namespace Andrei15193.Edesia
{
	public interface ILocalizationConfigElement
	{
		string LanguageDisplayName
		{
			get;
		}
		string LanguageId
		{
			get;
		}
		string ResourceFilesAssembly
		{
			get;
		}
		string EMailStringsResourceFile
		{
			get;
		}
		string ViewStringsResourceFile
		{
			get;
		}
		string ErrorStringsResourceFile
		{
			get;
		}
	}
}