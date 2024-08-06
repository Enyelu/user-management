using Microsoft.AspNetCore.Identity;
using user_management.domain.Entities;
using user_management.infrastructure;

namespace user_management.api.Extensions
{
    public static class ConfigureAppUser
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<AppUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoleManager<RoleManager<ApplicationRole>>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
        }
    }
}
