using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface IWishListService
{
    Task CreateAsync(long clientId);
    Task<List<WishListItemEntity>> GetAllWishListItemsAsync(long wishlistId);
    Task ClearAsync(long wishlistId);
}
