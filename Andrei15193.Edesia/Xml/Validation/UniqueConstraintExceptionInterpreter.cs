using System.Text.RegularExpressions;
using System.Xml.Schema;
namespace Andrei15193.Edesia.Xml.Validation
{
	public class UniqueConstraintExceptionInterpreter
		: IXmlSchemaExceptionInterpreter<UniqueConstraintException>
	{
		#region IXmlSchemaExceptionInterpreter Members
		public UniqueConstraintException Interpret(XmlSchemaException xmlSchemaException)
		{
			if (xmlSchemaException == null || xmlSchemaException.HResult != -2146231999)
				return null;

			Match errorMessageMatch = Regex.Match(xmlSchemaException.Message, "There is a duplicate key sequence '(.*)' for the '(.*)' key or unique identity constraint.");

			if (!errorMessageMatch.Success)
				return null;

			return new UniqueConstraintException(errorMessageMatch.Groups[1].Value, errorMessageMatch.Groups[2].Value, xmlSchemaException);
		}
		#endregion
	}
}