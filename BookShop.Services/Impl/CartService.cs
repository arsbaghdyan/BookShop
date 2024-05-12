using AutoMapper;
using BookShop.Data;
using BookShop.Data.Entities;
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

    public CartService(BookShopDbContext bookShopDbContext, ILogger<CartService> logger, IMapper mapper)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateAsync(long clientId)
    {
        var cart = await _bookShopDbContext.Carts.FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (cart != null)
        {
            _logger.LogInformation($"Cart with Id {cart.Id} is add for client with Id {clientId}");
        }

        var newCart = new CartEntity { ClientId = clientId };
        _bookShopDbContext.Carts.Add(newCart);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Cart with Id {newCart.Id} is add for client with Id {clientId}");
    }

    public async Task<List<CartItemModel>> GetAllCartItemsAsync(long cartId)
    {
        var cart = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
        {
            throw new ArgumentException("Cart not found");
        }

        var cartItems = _mapper.Map<List<CartItemModel>>(cart.CartItems);

        return cartItems;
    }

    public async Task ClearAsync(long cartId)
    {
        var cart = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
        {
            throw new ArgumentException("Cart not found");
        }

        _bookShopDbContext.CartItems.RemoveRange(cart.CartItems);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation("Cart items cleared successfully.");
    }
}