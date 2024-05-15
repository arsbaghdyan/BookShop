using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.Models.CartItemModels;
using AutoMapper;
using BookShop.Common.ClientService.Impl;
using BookShop.Common.ClientService.Abstractions;

namespace BookShop.Services.Impl;

internal class WishListService : IWishListService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public WishListService(BookShopDbContext bookShopDbContext, ILogger<WishListService> logger, IMapper mapper, IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<List<WishListItemModel>> GetAllWishListItemsAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var wishlist = await _bookShopDbContext.WishLists.Include(w => w.WishListItems).FirstOrDefaultAsync(w => w.ClientId == clientId);

        var wishlistItemModels = new List<WishListItemModel>();

        foreach (var wishlistItem in wishlist.WishListItems)
        {
            var wishlistItemModel = _mapper.Map<WishListItemModel>(wishlistItem);
            wishlistItemModels.Add(wishlistItemModel);
        }

        return wishlistItemModels;
    }

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var wishlist = await _bookShopDbContext.WishLists.Include(w => w.WishListItems).FirstOrDefaultAsync(w => w.ClientId == clientId);

        _bookShopDbContext.WishListItems.RemoveRange(wishlist.WishListItems);
        wishlist.WishListItems.Clear();
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation("WishList items cleared successfully.");
    }
}
