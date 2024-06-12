using BookShop.Common.ClientService.Abstractions;
using BookShop.Common.ClientService.Impl;
using BookShop.Common.EmployeeService.Abstractions;
using BookShop.Common.EmployeeService.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Common;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddClientContext(this IServiceCollection services)
    {
        services.AddScoped<ClientContext>();
        services.AddScoped<IClientContextWriter, ClientContextWriter>();
        services.AddScoped<IClientContextReader, ClientContextReader>();

        services.AddScoped<EmployeeContext>();
        services.AddScoped<IEmployeeContextWriter, EmployeeContextWriter>();
        services.AddScoped<IEmployeeContextReader, EmployeeContextReader>();

        return services;
    }
}
