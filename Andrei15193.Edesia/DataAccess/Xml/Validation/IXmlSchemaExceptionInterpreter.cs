using System.Xml.Schema;
namespace Andrei15193.Edesia.DataAccess.Xml.Validation
{
	public interface IXmlSchemaExceptionInterpreter<out TException>
		where TException : XmlSchemaException
	{
		TException Interpret(XmlSchemaException xmlSchemaException);
	}
}