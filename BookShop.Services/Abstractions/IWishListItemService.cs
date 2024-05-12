using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IWishListItemService
{
    Task AddAsync(WishListItemAddModel wishListItemEntity);
    Task RemoveAsync(long wishListItemId);
}
