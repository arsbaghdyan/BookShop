using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IWishListService
{
    Task CreateAsync(long clientId);
    Task<List<WishListItemGetVm>> GetAllWishListItemsAsync(long wishlistId);
    Task ClearAsync(long wishlistId);
}
