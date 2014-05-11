using System.Text.RegularExpressions;
using System.Xml.Schema;
namespace Andrei15193.Edesia.Xml.Validation
{
	public class XmlUniqueConstraintExceptionInterpreter
		: IXmlSchemaExceptionInterpreter<XmlUniqueConstraintException>
	{
		#region IXmlSchemaExceptionInterpreter Members
		public XmlUniqueConstraintException Interpret(XmlSchemaException xmlSchemaException)
		{
			if (xmlSchemaException == null || xmlSchemaException.HResult != -2146231999)
				return null;

			Match errorMessageMatch = Regex.Match(xmlSchemaException.Message, "There is a duplicate key sequence '(.*)' for the '(.*)' key or unique identity constraint.");

			if (!errorMessageMatch.Success)
				return null;

			return new XmlUniqueConstraintException(errorMessageMatch.Groups[1].Value, errorMessageMatch.Groups[2].Value, xmlSchemaException);
		}
		#endregion
	}
}