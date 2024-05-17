using BookShop.Common.ClientService.Abstractions;
using BookShop.Common.ClientService.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Common.ClientService;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClientContext(this IServiceCollection services)
    {
        services.AddScoped<ClientContext>();
        services.AddScoped<IClientContextWriter, ClientContextWriter>();
        services.AddScoped<IClientContextReader, ClientContextReader>();

        return services;
    }
}
