namespace Andrei15193.Edesia.Settings
{
	public interface ILanguageSettings
	{
		string LanguageDisplayName
		{
			get;
		}
		string LanguageId
		{
			get;
		}
		string ResourceFilesAssemblyName
		{
			get;
		}
		string EMailStringsResourceFile
		{
			get;
		}
		string ErrorStringsResourceFile
		{
			get;
		}
		string ViewStringsResourceFile
		{
			get;
		}
	}
}