﻿using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class CartService : ICartService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<CartService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public CartService(BookShopDbContext bookShopDbContext, ILogger<CartService> logger,
                       IMapper mapper, IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<List<CartItemModel>> GetAllCartItemsAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var cartEntity = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);

        var cartItemModels = new List<CartItemModel>();

        foreach (var cartItem in cartEntity.CartItems)
        {
            var cartItemModel = _mapper.Map<CartItemModel>(cartItem);
            cartItemModels.Add(cartItemModel);
        }

        return cartItemModels;
    }

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var cartEntity = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);

        _bookShopDbContext.CartItems.RemoveRange(cartEntity.CartItems);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"CartItems cleared successfully for client with id {clientId}.");
    }
}