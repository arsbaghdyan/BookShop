using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.Models.CartItemModels;
using AutoMapper;

namespace BookShop.Services.Impl;

internal class WishListService : IWishListService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListService> _logger;
    private readonly ICustomAuthenticationService _customAuthenticationService;
    private readonly IMapper _mapper;

    public WishListService(BookShopDbContext bookShopDbContext,
        ILogger<WishListService> logger, ICustomAuthenticationService customAuthenticationService, IMapper mapper)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
        _mapper = mapper;
    }

    public async Task CreateAsync(long clientId)
    {
        var wishlist = await _bookShopDbContext.WishLists.FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (wishlist == null)
        {
            throw new Exception("Wishlist not found");
        }

        _bookShopDbContext.WishLists.Add(wishlist);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Wishlist with Id {wishlist.Id} is add for client with Id {clientId}");
    }

    public async Task<List<WishListItemGetVm>> GetAllWishListItemsAsync(long wishlistId)
    {
        var wishlist = await _bookShopDbContext.WishLists.Include(c => c.WishListItems).FirstOrDefaultAsync(c => c.Id == wishlistId);

        if (wishlist == null)
        {
            throw new ArgumentException("Wishlist not found");
        }

        var wishlistItems = _mapper.Map<List<WishListItemGetVm>>(wishlist.WishListItems);

        return wishlistItems;
    }

    public async Task ClearAsync(long wishlistId)
    {
        var wishlist = await _bookShopDbContext.WishLists.Include(c => c.WishListItems).FirstOrDefaultAsync(c => c.Id == wishlistId);

        if (wishlist == null)
        {
            throw new ArgumentException("Cart not found");
        }

        _bookShopDbContext.WishListItems.RemoveRange(wishlist.WishListItems);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation("Cart items cleared successfully.");
    }
}
