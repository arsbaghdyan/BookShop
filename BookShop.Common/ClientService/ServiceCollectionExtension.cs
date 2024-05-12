﻿using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Common.ClientService;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClientContext(this IServiceCollection services)
    {
        services.AddScoped<ClientContext>();

        return services;
    }
}
