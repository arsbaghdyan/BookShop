using BookShop.Data.Entities;
using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.Models.CartItemModels;
using AutoMapper;
using BookShop.Common.ClientService.Impl;
using BookShop.Common.ClientService.Abstractions;

namespace BookShop.Services.Impl;

internal class CartItemService : ICartItemService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<CartItemService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public CartItemService(BookShopDbContext bookShopDbContext, ILogger<CartItemService> logger, IMapper mapper, ClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<CartItemModel> AddAsync(CartItemAddModel cartItem)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cart = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);

        var cartItemToAdd = _mapper.Map<CartItemEntity>(cartItem);

        cartItemToAdd.Id = cart.Id;

        cartItem.Price *= cartItem.Count;

        _bookShopDbContext.CartItems.Add(cartItemToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Cart with Id {cartItemToAdd.Id} added succesfully.");

        var cartItemModel = _mapper.Map<CartItemModel>(cartItemToAdd);

        return cartItemModel;
    }

    public async Task RemoveAsync(long cartItemId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cart = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);

        var cartEntity = cart.CartItems.FirstOrDefault(c => c.Id == cartItemId);

        _bookShopDbContext.CartItems.Remove(cartEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Cart with Id {cartEntity.Id} remove succesfully.");
    }

    public async Task<CartItemModel> UpdateAsync(CartItemUpdateModel cartItem)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cart = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);

        var cartEntity = cart.CartItems.FirstOrDefault(c => c.Id == cartItem.Id);

        cartEntity.Count = cartItem.Count;
        cartEntity.Price = cartItem.Price * cartItem.Count;

        cartEntity = _mapper.Map<CartItemEntity>(cartItem);

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"CartItem with Id {cartItem.Id} removed successfully");

        var cartItemModel = _mapper.Map<CartItemModel>(cart);

        return cartItemModel;
    }
}