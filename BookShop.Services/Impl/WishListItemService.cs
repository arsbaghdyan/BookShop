using BookShop.Data.Entities;
using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Impl;

internal class WishListItemService : IWishListItemService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListItemService> _logger;
    private readonly ICustomAuthenticationService _customAuthenticationService;

    public WishListItemService(BookShopDbContext bookShopDbContext, ILogger<WishListItemService> logger,
        ICustomAuthenticationService customAuthenticationService)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
    }

    public async Task AddAsync(WishListItemEntity wishListItemEntity)
    {
        if (wishListItemEntity == null)
        {
            throw new Exception("There is nothing to add");
        }

        var wishList = await _bookShopDbContext.WishLists.FirstOrDefaultAsync(c => c.Id == wishListItemEntity.WishListId);

        if (wishList == null)
        {
            throw new Exception("Wishlist not found");
        }

        var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == wishList.ClientId);

        if (client == null)
        {
            throw new Exception("Client not Found");
        }

        var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

        if (client.Email != checkingClientEmail)
        {
            throw new Exception("Unauthorized: You can only add your own wishlist.");
        }

        if (wishList.WishListItems == null)
        {
            wishList.WishListItems = new List<WishListItemEntity>();
        }

        _bookShopDbContext.WishListItems.Add(wishListItemEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Wishlist with Id {wishListItemEntity.Id} added succesfully.");
    }

    public async Task RemoveAsync(WishListItemEntity wishListItemEntity)
    {
        if (wishListItemEntity == null)
        {
            throw new Exception("There is nothing to add");
        }

        var wishlist = await _bookShopDbContext.WishLists.FirstOrDefaultAsync(c => c.Id == wishListItemEntity.WishListId);

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
            throw new Exception("Unauthorized: You can only remove your own wishlist.");
        }

        if (wishlist.WishListItems == null)
        {
            throw new Exception("Wishlist is Empty");
        }

        _bookShopDbContext.WishListItems.Remove(wishListItemEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Wishlist with Id {wishListItemEntity.Id} remove succesfully.");
    }
}
