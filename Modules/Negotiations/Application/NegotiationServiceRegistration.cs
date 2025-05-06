using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Negotiations.Application.Services;
using Negotiations.Domain.Interfaces;

namespace Negotiations.Application;

public static class NegotiationsServiceRegistration
{
    public static IServiceCollection AddNegotiationsApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped<INegotiationService, NegotiationService>();

        return services;
    }
}