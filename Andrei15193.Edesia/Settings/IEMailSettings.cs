using System.Net;
using System.Net.Mail;
namespace Andrei15193.Edesia.Settings
{
	public interface IEMailSettings
	{
		string SmtpHost
		{
			get;
		}
		int SmtpPort
		{
			get;
		}
		NetworkCredential Credentials
		{
			get;
		}
		MailAddress SenderMailAddress
		{
			get;
		}
	}
}