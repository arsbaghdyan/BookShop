using BookShop.Services.Abstractions;
using BookShop.Services.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllServices(this IServiceCollection services)
    {
        services.AddTransient<IClientService, ClientService>();
        services.AddTransient<IProductService, ProductService>();

        return services;
    }
}
