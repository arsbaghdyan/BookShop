using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IWishListService
{
    Task<List<WishListItemModel>> GetAllWishListItemsAsync();
    Task<WishListItemModel> AddAsync(WishListItemAddModel wishListItemEntity);
    Task RemoveAsync(long wishListItemId);
    Task ClearAsync();
}
