using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;

namespace Shared
{
    public static class SharedModule
    {
        public static IServiceCollection AddSharedModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedInfrastructure();
            
            return services;
        }
    }
}