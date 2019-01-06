using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BE
{
	public class MailClient
	{
		SmtpClient client;

		public MailClient()
		{
			client = new SmtpClient();
			client.Credentials = new NetworkCredential("CsharpProject5779@gmail.com", "GoldShmid");
			client.Host = "smtp.gmail.com";
			client.Port = 587;
			client.EnableSsl = true;
			
		}
		public void send(MailMessage message)
		{
			client.Send(message);
		}
		
	}
}
