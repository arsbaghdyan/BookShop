using BookShop.Data.Entities;
using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.Models.CartItemModels;
using AutoMapper;

namespace BookShop.Services.Impl;

internal class CartItemService : ICartItemService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<CartItemService> _logger;
    private readonly IMapper _mapper;

    public CartItemService(BookShopDbContext bookShopDbContext, ILogger<CartItemService> logger, IMapper mapper)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task AddAsync(CartItemAddVm cartItem)
    {
        if (cartItem == null)
        {
            throw new Exception("There is nothing to add");
        }

        cartItem.Price = cartItem.Price * cartItem.Count;

        var cartItemToAdd = _mapper.Map<CartItemEntity>(cartItem);

        _bookShopDbContext.CartItems.Add(cartItemToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Cart with Id {cartItemToAdd.Id} added succesfully.");
    }

    public async Task RemoveAsync(long cartItemId)
    {
        var cartItem = await _bookShopDbContext.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);

        if (cartItem == null)
        {
            throw new Exception("Cart is Empty");
        }

        _bookShopDbContext.CartItems.Remove(cartItem);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Cart with Id {cartItem.Id} remove succesfully.");
    }

    public async Task UpdateAsync(CartItemUpdateVm cartItem)
    {
        if (cartItem == null)
        {
            throw new Exception("CartItem not exist");
        }

        var cartItemToUpdate = _bookShopDbContext.CartItems.FirstOrDefault(c => c.Id == cartItem.Id);

        if (cartItemToUpdate == null)
        {
            throw new Exception("CartItem not found");
        }

        cartItemToUpdate.Count = cartItem.Count;
        cartItemToUpdate.Price = cartItem.Price * cartItem.Count;

        cartItemToUpdate = _mapper.Map<CartItemEntity>(cartItem);

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"CartItem with Id {cartItem.Id} removed from Cart with Id {cartItemToUpdate.Id} ");
    }
}