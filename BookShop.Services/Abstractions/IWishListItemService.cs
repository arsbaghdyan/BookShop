using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IWishListItemService
{
    Task<WishListItemModel> AddAsync(WishListItemAddModel wishListItemEntity);
    Task RemoveAsync(long wishListItemId);
}
