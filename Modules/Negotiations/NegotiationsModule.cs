using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Negotiations.Application;
using Negotiations.Infrastructure;

namespace Negotiations;

public static class NegotiationsModule
{
    public static IServiceCollection AddNegotiationsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddNegotiationsInfrastructure(configuration);
        services.AddNegotiationsApplication();

        return services;
    }
}