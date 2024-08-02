using user_management.domain.Models;

namespace user_management.infrastructure.Services.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(EmailRequest emailRequest);
    }
}
