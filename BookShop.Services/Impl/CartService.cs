using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class CartService : ICartService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<CartService> _logger;
    private readonly ICustomAuthenticationService _customAuthenticationService;

    public CartService(BookShopDbContext bookShopDbContext, ILogger<CartService> logger, ICustomAuthenticationService customAuthenticationService)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
    }

    public async Task CreateAsync(long clientId)
    {
        try
        {
            var cart = await _bookShopDbContext.Carts.FirstOrDefaultAsync(c => c.Id == clientId);

            if (cart != null)
            {
                throw new Exception("Cart for client already exist");
            }

            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can not create cart for other client.");
            }

            var cartToAdd = new CartEntity { ClientId = clientId };
            _bookShopDbContext.Carts.Add(cartToAdd);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Cart with Id {cartToAdd.Id} is add for client with Id {clientId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task<List<CartItemEntity>> GetAllCartItemsAsync(long cartId)
    {
        try
        {
            var cart = await _bookShopDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartId);

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
                throw new Exception("Unauthorized: You can not create cart for other client.");
            }

            return cart.CartItems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task ClearAsync(long cartId)
    {
        try
        {
            var cart = await _bookShopDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartId);

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
                throw new Exception("Unauthorized: You can only clear your own cart.");
            }

            if (cart.CartItems == null)
            {
                throw new Exception("Cart is Empty");
            }

            _bookShopDbContext.CartItems.RemoveRange(cart.CartItems);
            cart.CartItems.Clear();
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation("CartItem cleared successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while clearing cartItem.");
            throw;
        }
    }
}