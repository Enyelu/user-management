using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using user_management.domain.Models;
using user_management.infrastructure.Services.Interfaces;
using user_management.infrastructure.Shared;

namespace user_management.infrastructure.Services.Implementations
{
    public class MailService : IMailService
    {
        private readonly HttpClient _client;
        private readonly Settings _settings;
        public MailService(HttpClient client, IOptions<Settings> options)
        {
            _client = client;
            _settings = options.Value;
        }
        public async Task<bool> SendMailAsync(EmailRequest emailRequest)
        {
            var emailPayload = new
            {
                sender = new
                {
                    name = _settings.EmailSenderName,
                    email = _settings.EmailSenderEmail,
                },
                to = new[]
                {
                    new { email = emailRequest.ToEmail}
                },
                subject = emailRequest.Subject,
                htmlContent = emailRequest.Body,
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(emailPayload), Encoding.UTF8,"application/json");

            // Set the request headers
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("api-key", _settings.EmailSenderAppKey);

            // Send the POST request
            var response = await _client.PostAsync("", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
