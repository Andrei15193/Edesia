using System;
namespace Andrei15193.Edesia.Exceptions
{
	public class UniqueStreetException
		: UniqueConstraintException
	{
		public UniqueStreetException(string conflictingValue, Exception innerException = null)
			: base(string.Format("Duplicate '{0}' value", conflictingValue), innerException)
		{
			_conflictingValue = conflictingValue;
		}

		public override string ConstrainName
		{
			get
			{
				return "Unique Street";
			}
		}
		public override string ConflictingValue
		{
			get
			{
				return _conflictingValue;
			}
		}

		private readonly string _conflictingValue;
	}
}