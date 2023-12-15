using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net.Mail;

namespace WebApplication2.Services
{
	public class MailService
	{
		public void SendMail(string to, string subject, string body)
		{
			using (var client = new SmtpClient())
			{
				
			}
		}
	}
}
