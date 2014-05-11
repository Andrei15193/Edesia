using System;
namespace Andrei15193.Edesia.Exceptions
{
	public class UniqueDeliveryZoneNameException
		: UniqueConstraintException
	{
		public UniqueDeliveryZoneNameException(string conflictingValue, Exception innerException = null)
			: base(string.Format("Duplicate '{0}' value", conflictingValue), innerException)
		{
			_conflictingValue = conflictingValue;
		}

		public override string ConstrainName
		{
			get
			{
				return "Unique Delivery Zone";
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