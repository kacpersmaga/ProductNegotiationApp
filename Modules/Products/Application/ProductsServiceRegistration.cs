using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Products.Application;

public static class ProductsServiceRegistration
{
    public static IServiceCollection AddProductsApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}