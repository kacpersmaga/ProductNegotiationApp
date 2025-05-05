using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Products.Application;

public static class ProductsServiceRegistration
{
    public static IServiceCollection AddProductsApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}