using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Application;
using Products.Infrastructure;

namespace Products;

public static class ProductsModule
{
    public static IServiceCollection AddProductsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProductsInfrastructure(configuration);
        services.AddProductsApplication();

        return services;
    }
}