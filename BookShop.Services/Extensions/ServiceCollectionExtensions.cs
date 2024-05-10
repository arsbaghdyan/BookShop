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
        services.AddTransient<ICustomAuthenticationService, CustomAuthenticationService>();
        services.AddTransient<ICartService, CartService>();
        services.AddTransient<ICartItemService, CartItemService>();
        services.AddTransient<IWishListService, WishListService>();
        services.AddTransient<IWishListItemService, WishListItemService>();
        services.AddTransient<IPaymentMethodService, PaymentMethodService>();

        return services;
    }
}
