using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Interfaces;
using Shared.Infrastructure.Services;

namespace Shared.Infrastructure
{
    public static class SharedInfrastructureRegistration
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, DateTimeService>();

            return services;
        }
    }
}