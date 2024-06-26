﻿using BookShop.Services.Models.WishListItemModels;

namespace BookShop.Services.Abstractions;

public interface IWishListService
{
    Task<List<WishListItemModel?>> GetAllWishListItemsAsync();
    Task<WishListItemModel?> AddAsync(WishListItemAddModel wishListItemEntity);
    Task RemoveAsync(long productId);
    Task ClearAsync();
}
