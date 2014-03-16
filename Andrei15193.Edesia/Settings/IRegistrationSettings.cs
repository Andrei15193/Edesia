namespace Andrei15193.Edesia.Settings
{
	public interface IRegistrationSettings
	{
		int RegistrationKeyLength
		{
			get;
		}
		double RegistrationKeyHoursTimeout
		{
			get;
		}
	}
}