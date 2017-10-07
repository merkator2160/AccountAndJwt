using AccountAndJwt.Models.Service;
using System.Threading.Tasks;

namespace AccountAndJwt.Services.Interfaces
{
    public interface IEmailService
    {
        void Send(EmailMessage message);
        Task SendAsync(EmailMessage message);
    }
}