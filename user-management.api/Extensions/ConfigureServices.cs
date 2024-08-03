using user_management.infrastructure.Services.Implementations;
using user_management.infrastructure.Services.Interfaces;

namespace user_management.api.Extensions
{
    public static class ConfigureServices
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IMailService, MailService>();
            services.AddHttpClient<IMailService, MailService>(client =>
            {
                client.BaseAddress = new Uri(config["Settings:EmailSenderUrl"]);
            });
        }
    }
}
