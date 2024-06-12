using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data.Entities;
using BookShop.Services.Models.WishListItemModels;

namespace BookShop.Services.Impl;

internal class WishListService : IWishListService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public WishListService(BookShopDbContext bookShopDbContext, 
                           ILogger<WishListService> logger,
                           IMapper mapper, 
                           IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<List<WishListItemModel?>> GetAllWishListItemsAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var wishListEntity = await _bookShopDbContext.WishLists.Include(w => w.WishListItems).FirstOrDefaultAsync(w => w.ClientId == clientId);

        var wishListItemModels = new List<WishListItemModel?>();

        foreach (var wishlistItem in wishListEntity.WishListItems)
        {
            var wishListItemModel = _mapper.Map<WishListItemModel?>(wishlistItem);
            wishListItemModels.Add(wishListItemModel);
        }

        return wishListItemModels;
    }

    public async Task<WishListItemModel?> AddAsync(WishListItemAddModel wishListItem)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var wishListEntity = await _bookShopDbContext.WishLists
            .Include(w => w.WishListItems)
            .FirstOrDefaultAsync(w => w.ClientId == clientId);

        var wishListItemToAdd = _mapper.Map<WishListItemEntity?>(wishListItem);

        wishListItemToAdd.WishListId = wishListEntity.Id;

        _bookShopDbContext.WishListItems.Add(wishListItemToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"WishList with Id {wishListItemToAdd.Id} added succesfully for '{clientId}' client.");

        return _mapper.Map<WishListItemModel?>(wishListItemToAdd);
    }

    public async Task RemoveAsync(long productId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        await _bookShopDbContext.WishListItems
            .Where(c => c.WishList.ClientId == clientId && c.ProductId == productId)
            .ExecuteDeleteAsync();

        _logger.LogInformation($"Product with {productId} Id is succesfully removed from WishList for '{clientId}' client.");
    }

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        await _bookShopDbContext.WishListItems
            .Where(c => c.WishList.ClientId == clientId)
            .ExecuteDeleteAsync();

        _logger.LogInformation($"WishList items cleared successfully for '{clientId}' client.");
    }
}
