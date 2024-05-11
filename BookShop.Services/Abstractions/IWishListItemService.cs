using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IWishListItemService
{
    Task AddAsync(WishListItemAddVm wishListItemEntity);
    Task RemoveAsync(long wishListItemId);
}
