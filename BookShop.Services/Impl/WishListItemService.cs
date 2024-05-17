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

    public WishListItemService(BookShopDbContext bookShopDbContext, ILogger<WishListItemService> logger, 
                               IMapper mapper, IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<WishListItemModel> AddAsync(WishListItemAddModel wishListItem)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var wishListEntity = await _bookShopDbContext.WishLists.Include(w => w.WishListItems).FirstOrDefaultAsync(w => w.ClientId == clientId);

        var wishListItemToAdd = _mapper.Map<WishListItemEntity>(wishListItem);

        wishListItemToAdd.WishListId = wishListEntity.Id;

        _bookShopDbContext.WishListItems.Add(wishListItemToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"WishList with Id {wishListItemToAdd.Id} added succesfully for client with id {clientId}.");

        var wishListItemModel = _mapper.Map<WishListItemModel>(wishListItemToAdd);

        return wishListItemModel;
    }

    public async Task RemoveAsync(long wishListItemId)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var wishListEntity = await _bookShopDbContext.WishLists.Include(w => w.WishListItems).FirstOrDefaultAsync(w => w.ClientId == clientId);
        var wishListItemEntity = wishListEntity.WishListItems.FirstOrDefault(w => w.Id == wishListItemId);

        _bookShopDbContext.WishListItems.Remove(wishListItemEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"WishList with Id {wishListEntity.Id} remove succesfully for client with id {clientId}.");
    }
}
