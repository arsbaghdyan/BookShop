using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.Models.CartItemModels;
using AutoMapper;
using BookShop.Common.ClientService;

namespace BookShop.Services.Impl;

internal class WishListService : IWishListService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListService> _logger;
    private readonly IMapper _mapper;
    private readonly ClientContextReader _clientContextReader;

    public WishListService(BookShopDbContext bookShopDbContext, ILogger<WishListService> logger, IMapper mapper, ClientContextReader clientContextReader)
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

        if (wishlist == null)
        {
            throw new ArgumentException("Wishlist not found");
        }

        var wishlistItems = _mapper.Map<List<WishListItemModel>>(wishlist.WishListItems);

        return wishlistItems;
    }

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var wishlist = await _bookShopDbContext.WishLists.Include(w => w.WishListItems).FirstOrDefaultAsync(w => w.ClientId == clientId);

        if (wishlist == null)
        {
            throw new ArgumentException("WishList not found");
        }

        _bookShopDbContext.WishListItems.RemoveRange(wishlist.WishListItems);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation("Cart items cleared successfully.");
    }
}
