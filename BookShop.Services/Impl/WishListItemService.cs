using BookShop.Data.Entities;
using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Impl;

internal class WishListItemService : IWishListItemService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListItemService> _logger;
    private readonly IMapper _mapper;

    public WishListItemService(BookShopDbContext bookShopDbContext, ILogger<WishListItemService> logger, IMapper mapper)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task AddAsync(WishListItemAddModel wishListItem)
    {
        if (wishListItem == null)
        {
            throw new Exception("There is nothing to add");
        }

        var wishlistItemToAdd = _mapper.Map<WishListItemEntity>(wishListItem);

        _bookShopDbContext.WishListItems.Add(wishlistItemToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Wishlist with Id {wishlistItemToAdd.Id} added succesfully.");
    }

    public async Task RemoveAsync(long wishlistId)
    {
        var wishlist = await _bookShopDbContext.WishListItems.FirstOrDefaultAsync(c => c.Id == wishlistId);

        if (wishlist == null)
        {
            throw new Exception("Wishlist not found");
        }

        _bookShopDbContext.WishListItems.Remove(wishlist);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Wishlist with Id {wishlist.Id} remove succesfully.");
    }
}
