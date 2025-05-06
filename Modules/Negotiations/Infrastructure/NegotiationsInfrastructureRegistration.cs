using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Negotiations.Infrastructure.Persistence;
using Negotiations.Infrastructure.Repositories;

namespace Negotiations.Infrastructure;

public static class NegotiationsInfrastructureRegistration
{
    public static IServiceCollection AddNegotiationsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NegotiationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        return services;
    }
}