using System;
namespace Andrei15193.Edesia.Xml.Validation
{
	public class XmlUniqueConstraintException
		: XmlSchemaConstraintException
	{
		public XmlUniqueConstraintException(string conflictingValue, string constraintName, Exception innerException = null)
			: base(string.Format("There is a duplicate key sequence '{0}' for the '{1}' key or unique identity constraint.", conflictingValue, constraintName), innerException)
		{
			if (conflictingValue == null)
				throw new ArgumentNullException("conflictingValue");
			if (constraintName == null)
				throw new ArgumentNullException("constraintName");
			if (string.IsNullOrEmpty(constraintName) || string.IsNullOrWhiteSpace(constraintName))
				throw new ArgumentException("Cannot be empty or whitespace!", "constraintName");

			_conflictingValue = conflictingValue;
			_constraintName = constraintName;
		}

		public override string ConstraintType
		{
			get
			{
				return "UniqueConstraint";
			}
		}
		public override string ConflictingValue
		{
			get
			{
				return _conflictingValue;
			}
		}
		public override string ConstraintName
		{
			get
			{
				return _constraintName;
			}
		}

		private readonly string _conflictingValue;
		private readonly string _constraintName;
	}
}