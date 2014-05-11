using System;
namespace Andrei15193.Edesia.Exceptions
{
	public class UniqueAddressException
		: UniqueConstraintException
	{
		public UniqueAddressException(string conflictingValue, Exception innerException = null)
			: base(string.Format("Duplicate '{0}' value", conflictingValue), innerException)
		{
			_conflictingValue = conflictingValue;
		}

		public override string ConstrainName
		{
			get
			{
				return "Unique Address";
			}
		}
		public override string ConflictingValue
		{
			get
			{
				return _conflictingValue;
			}
		}

		private string _conflictingValue;
	}
}