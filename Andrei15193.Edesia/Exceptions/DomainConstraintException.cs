using System;
namespace Andrei15193.Edesia.Exceptions
{
	public abstract class DomainConstraintException
		: ApplicationException
	{
		protected DomainConstraintException(string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}