using AccountAndJwt.Middleware.Configs;
using AccountAndJwt.Services.Interfaces;
using AccountAndJwt.Services.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AccountAndJwt.Services
{
    internal class BasicEmailService : IEmailService
    {
        private readonly EmailServiceConfig _config;


        public BasicEmailService(IOptions<EmailServiceConfig> emailServiceConfig)
        {
            _config = emailServiceConfig.Value;
        }


        // IEmailService //////////////////////////////////////////////////////////////////////////
        public void Send(EmailMessage message)
        {
            using (var client = new SmtpClient(_config.SmtpUri, _config.Port)
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
            using (var client = new SmtpClient(_config.SmtpUri, _config.Port)
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