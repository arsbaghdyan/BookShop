﻿using BookShop.Services.Abstractions;
using BookShop.Services.Impl;
using BookShop.Services.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICartItemService, CartItemService>();
        services.AddScoped<IWishListService, WishListService>();
        services.AddScoped<IWishListItemService, WishListItemService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<TokenModel>();

        return services;
    }
}
