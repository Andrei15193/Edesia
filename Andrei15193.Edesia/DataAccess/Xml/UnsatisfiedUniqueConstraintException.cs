using System;
using System.Xml.Schema;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public class UnsatisfiedUniqueConstraintException
		: XmlSchemaException
	{
		public UnsatisfiedUniqueConstraintException(string conflictingValue, string unsatisfiedConstraint = null)
			: base(string.Format("There is a duplicate key sequence '{0}' for the '{1}' key or unique identity constraint.", conflictingValue, unsatisfiedConstraint))
		{
			if (conflictingValue == null)
				throw new ArgumentNullException("conflictingValue");
			_conflictingValue = conflictingValue;
			_unsatisfiedConstraint = unsatisfiedConstraint;
		}

		public string ConflictingValue
		{
			get
			{
				return _conflictingValue;
			}
		}
		public string UnsatisfiedConstraint
		{
			get
			{
				return _unsatisfiedConstraint;
			}
		}

		private readonly string _conflictingValue;
		private readonly string _unsatisfiedConstraint;
	}
}