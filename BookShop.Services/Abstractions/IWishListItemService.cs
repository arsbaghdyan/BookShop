using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface IWishListItemService
{
    Task AddAsync(WishListItemEntity wishListItemEntity);
    Task RemoveAsync(WishListItemEntity wishListItemEntity);
}
