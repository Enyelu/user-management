using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace user_management.core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationCore(this IServiceCollection services)
        {
            services.AddMediatR(m => m.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
