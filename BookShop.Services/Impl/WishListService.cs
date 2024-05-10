using BookShop.Data.Entities;
using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Impl;

internal class WishListService : IWishListService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListService> _logger;
    private readonly ICustomAuthenticationService _customAuthenticationService;

    public WishListService(BookShopDbContext bookShopDbContext,
        ILogger<WishListService> logger, ICustomAuthenticationService customAuthenticationService)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
    }

    public async Task CreateAsync(long clientId)
    {
        try
        {
            var wishlist = await _bookShopDbContext.WishLists.FirstOrDefaultAsync(c => c.Id == clientId);

            if (wishlist != null)
            {
                throw new Exception("Wishlist for client already exist");
            }

            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can not create wishlist for other client.");
            }

            var wishlistToAdd = new WishListEntity { ClientId = clientId };
            _bookShopDbContext.WishLists.Add(wishlistToAdd);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Wishlist with Id {wishlistToAdd.Id} is add for client with Id {clientId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task<List<WishListItemEntity>> GetAllWishListItemsAsync(long wishlistId)
    {
        try
        {
            var wishlist = await _bookShopDbContext.WishLists.FirstOrDefaultAsync(c => c.Id == wishlistId);

            if (wishlist == null)
            {
                throw new Exception("Wishlist not found");
            }

            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == wishlist.ClientId);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can not create wishlist for other client.");
            }

            var listToReturn = new List<WishListItemEntity>();
            var listItems = await _bookShopDbContext.WishListItems.Where(c => c.WishListId == wishlistId).ToListAsync();

            listToReturn.AddRange(listItems);

            return listToReturn;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task ClearAsync(long wishlistId)
    {
        try
        {
            var wishlist = await _bookShopDbContext.WishLists.FirstOrDefaultAsync(c => c.Id == cartId);

            if (wishlist == null)
            {
                throw new Exception("Wishlist not found");
            }

            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == wishlist.ClientId);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can only clear your own cart.");
            }

            if (wishlist.WishListItems == null)
            {
                throw new Exception("Wishlist is Empty");
            }

            _bookShopDbContext.WishListItems.RemoveRange(wishlist.WishListItems);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation("WishlistItems cleared successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }
}
