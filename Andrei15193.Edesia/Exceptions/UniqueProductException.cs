using System;
namespace Andrei15193.Edesia.Exceptions
{
	public class UniqueProductException
		: UniqueConstraintException
	{
		public UniqueProductException(string conflictingValue, Exception innerException = null)
			: base(string.Format("Duplicate '{0}' value", conflictingValue), innerException)
		{
			_conflictingValue = conflictingValue;
		}

		public override string ConstrainName
		{
			get
			{
				return "Unique Product";
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