using System.IO;
using System.Net;
using System.Net.Mail;
namespace Andrei15193.Edesia.Settings.Local
{
	public sealed class LocalEMailSettings
		: IEMailSettings
	{
		public LocalEMailSettings(string fileName)
		{
			using (StreamReader streamReader = new StreamReader(fileName))
			{
				_smtpHost = streamReader.ReadLine();
				_smtpPort = int.Parse(streamReader.ReadLine());
				_credentials = new NetworkCredential(streamReader.ReadLine(), streamReader.ReadLine());
				_senderMailAddress = new MailAddress(streamReader.ReadLine(), streamReader.ReadLine());
			}
		}

		#region IEmailSettings Members
		public string SmtpHost
		{
			get
			{
				return _smtpHost;
			}
		}
		public int SmtpPort
		{
			get
			{
				return _smtpPort;
			}
		}
		public NetworkCredential Credentials
		{
			get
			{
				return _credentials;
			}
		}
		public MailAddress SenderMailAddress
		{
			get
			{
				return _senderMailAddress;
			}
		}
		#endregion

		private readonly string _smtpHost;
		private readonly int _smtpPort;
		private readonly NetworkCredential _credentials;
		private readonly MailAddress _senderMailAddress;
	}
}