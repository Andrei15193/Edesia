using System;
namespace Andrei15193.Edesia.Exceptions
{
	public abstract class UniqueConstraintException
		: DomainConstraintException
	{
		protected UniqueConstraintException(string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}

		public abstract string ConstrainName
		{
			get;
		}
		public abstract string ConflictingValue
		{
			get;
		}
	}
}