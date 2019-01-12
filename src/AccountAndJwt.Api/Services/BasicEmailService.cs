using AccountAndJwt.Api.Middleware.Config.Models;
using AccountAndJwt.Api.Services.Interfaces;
using AccountAndJwt.Api.Services.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Services
{
	internal sealed class BasicEmailService : IEmailService
	{
		private readonly EmailServiceConfig _config;


		public BasicEmailService(EmailServiceConfig emailServiceConfig)
		{
			_config = emailServiceConfig;
		}


		// IEmailService //////////////////////////////////////////////////////////////////////////
		public void Send(EmailMessage message)
		{
			using(var client = new SmtpClient(_config.SmtpUri, _config.Port)
			{
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_config.Login, _config.Password),
				EnableSsl = _config.EnabledSsl
			})
			{
				client.Send(new MailMessage(_config.Login, message.Destination)
				{
					Subject = message.Subject,
					Body = message.Body,
					IsBodyHtml = true
				});
			}
		}
		public async Task SendAsync(EmailMessage message)
		{
			using(var client = new SmtpClient(_config.SmtpUri, _config.Port)
			{
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_config.Login, _config.Password),
				EnableSsl = _config.EnabledSsl
			})
			{
				await client.SendMailAsync(new MailMessage(_config.Login, message.Destination)
				{
					Subject = message.Subject,
					Body = message.Body,
					IsBodyHtml = true
				});
			}
		}
	}
}