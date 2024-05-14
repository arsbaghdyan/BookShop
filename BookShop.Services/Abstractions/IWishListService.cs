using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IWishListService
{
    Task<List<WishListItemModel>> GetAllWishListItemsAsync();
    Task ClearAsync();
}
