using BookShop.Data.Entities;
using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BookShop.Services.Models.CartItemModels;
using BookShop.Common.ClientService.Abstractions;

namespace BookShop.Services.Impl;

internal class WishListItemService : IWishListItemService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListItemService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public WishListItemService(BookShopDbContext bookShopDbContext, ILogger<WishListItemService> logger, IMapper mapper, IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<WishListItemModel> AddAsync(WishListItemAddModel wishListItem)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var wishList = await _bookShopDbContext.WishLists.Include(w => w.WishListItems).FirstOrDefaultAsync(w => w.ClientId == clientId);

        var wishlistItemToAdd = _mapper.Map<WishListItemEntity>(wishListItem);

        wishlistItemToAdd.WishListId = wishList.Id;

        _bookShopDbContext.WishListItems.Add(wishlistItemToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Wishlist with Id {wishlistItemToAdd.Id} added succesfully for client with id {clientId}.");

        var wishListItemModel = _mapper.Map<WishListItemModel>(wishlistItemToAdd);

        return wishListItemModel;
    }

    public async Task RemoveAsync(long wishlistItemId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var wishlist = await _bookShopDbContext.WishLists.Include(w => w.WishListItems).FirstOrDefaultAsync(w => w.ClientId == clientId);

        var wishListEntity = wishlist.WishListItems.FirstOrDefault(w => w.Id == wishlistItemId);

        _bookShopDbContext.WishListItems.Remove(wishListEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Wishlist with Id {wishlist.Id} remove succesfully for client with id {clientId}.");
    }
}
