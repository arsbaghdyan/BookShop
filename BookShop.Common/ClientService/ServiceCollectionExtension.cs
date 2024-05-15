using BookShop.Common.ClientService.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Common.ClientService;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClientContext(this IServiceCollection services)
    {
        services.AddScoped<ClientContext>();
        services.AddScoped<ClientContextWriter>();
        services.AddScoped<ClientContextReader>();

        return services;
    }
}
