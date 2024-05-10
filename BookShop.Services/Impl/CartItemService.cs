using BookShop.Data.Entities;
using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Impl;

internal class CartItemService : ICartItemService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<CartItemService> _logger;
    private readonly ICustomAuthenticationService _customAuthenticationService;

    public CartItemService(BookShopDbContext bookShopDbContext, ILogger<CartItemService> logger,
        ICustomAuthenticationService customAuthenticationService)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
    }

    public async Task AddAsync(CartItemEntity cartItem)
    {
        try
        {
            if (cartItem == null)
            {
                throw new Exception("There is nothing to add");
            }

            var cart = await _bookShopDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartItem.CartId);

            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == cart.ClientId);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can only add your own cart.");
            }

            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItemEntity>();
            }

            cartItem.Price = cartItem.Price * cartItem.Count;
            _bookShopDbContext.Add(cartItem);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Cart with Id {cartItem.Id} added succesfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task RemoveAsync(CartItemEntity cartItem)
    {
        try
        {
            if (cartItem == null)
            {
                throw new Exception("There is nothing to add");
            }

            var cart = await _bookShopDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartItem.CartId);

            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == cart.ClientId);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can only remove your own cart.");
            }

            if (cart.CartItems == null)
            {
                throw new Exception("Cart is Empty");
            }

            _bookShopDbContext.Remove(cartItem);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Cart with Id {cartItem.Id} remove succesfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(CartItemEntity cartItemEntity)
    {
        try
        {
            if (cartItemEntity == null)
            {
                throw new Exception("CartItem not exist");
            }

            var cart = await _bookShopDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartItemEntity.CartId);

            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == cart.Id);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can not create cart for other client.");
            }

            var cartItemToUpdate = _bookShopDbContext.CartItems.FirstOrDefault(c => c.Id == cartItemEntity.Id);

            if (cartItemToUpdate == null)
            {
                throw new Exception("CartItem not found");
            }

            cartItemToUpdate.Count = cartItemEntity.Count;
            cartItemToUpdate.Price = cartItemEntity.Price * cartItemEntity.Count;

            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"CartItem with Id {cartItemEntity.Id} removed from Cart with Id {cart.Id} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }
}
