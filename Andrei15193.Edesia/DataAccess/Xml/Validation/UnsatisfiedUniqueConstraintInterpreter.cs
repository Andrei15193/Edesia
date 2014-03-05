using System.Text.RegularExpressions;
using System.Xml.Schema;
namespace Andrei15193.Edesia.DataAccess.Xml.Validation
{
	public class UnsatisfiedUniqueConstraintInterpreter
		: IXmlSchemaExceptionInterpreter<UnsatisfiedUniqueConstraintException>
	{
		#region IXmlSchemaExceptionInterpreter Members
		public UnsatisfiedUniqueConstraintException Interpret(XmlSchemaException xmlSchemaException)
		{
			if (xmlSchemaException == null || xmlSchemaException.HResult != -2146231999)
				return null;

			Match errorMessageMatch = Regex.Match(xmlSchemaException.Message, "There is a duplicate key sequence '(.*)' for the '(.*)' key or unique identity constraint.");
			return new UnsatisfiedUniqueConstraintException(errorMessageMatch.Groups[1].Value, errorMessageMatch.Groups[2].Value);
		}
		#endregion
	}
}