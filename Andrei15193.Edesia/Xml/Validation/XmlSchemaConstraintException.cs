using System;
using System.Xml.Schema;
namespace Andrei15193.Edesia.Xml.Validation
{
	public abstract class XmlSchemaConstraintException
		: XmlSchemaException
	{
		protected XmlSchemaConstraintException(string message, Exception innerException = null)
			: base(message, innerException)
		{
		}

		public abstract string ConstraintType
		{
			get;
		}
		public abstract string ConstraintName
		{
			get;
		}
		public abstract string ConflictingValue
		{
			get;
		}
	}
}