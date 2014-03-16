namespace Andrei15193.Edesia.Settings
{
	public sealed class RegistrationSettings
		: IRegistrationSettings
	{
		public RegistrationSettings(int registrationKeyLength, double registrationKeyHoursTimeout)
		{
			_registrationKeyLength = registrationKeyLength;
			_registrationKeyHoursTimeout = registrationKeyHoursTimeout;
		}

		#region IRegistrationSettings Members
		public int RegistrationKeyLength
		{
			get
			{
				return _registrationKeyLength;
			}
		}
		public double RegistrationKeyHoursTimeout
		{
			get
			{
				return _registrationKeyHoursTimeout;
			}
		}
		#endregion

		private readonly int _registrationKeyLength;
		private readonly double _registrationKeyHoursTimeout;
	}
}