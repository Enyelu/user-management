using user_management.domain.Models;
using user_management.infrastructure.Services.Interfaces;

namespace user_management.infrastructure.Services.Implementations
{
    public class MailService : IMailService
    {
        public async Task<bool> SendMailAsync(EmailRequest emailRequest)
        {
            return true;
        }
    }
}
