using Microsoft.EntityFrameworkCore;
using user_management.infrastructure;

namespace user_management.api.Extensions
{
    public static class ConfigureDatabase
    {
        public static void ConfigureApplicationDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("default"));
            });
        }
    }
}
