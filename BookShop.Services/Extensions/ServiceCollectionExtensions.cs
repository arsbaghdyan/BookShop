using BookShop.Services.Abstractions;
using BookShop.Services.Impl;
using BookShop.Services.Mock;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IShopAuthenticationService, ShopAuthenticationService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IWishListService, WishListService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IInvoiceService, InvoiceService>();

        services.AddSingleton<IBillingService, MockBillingService>();

        return services;
    }
}
