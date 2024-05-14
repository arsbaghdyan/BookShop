using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface ICartItemService
{
    Task<CartItemModel> AddAsync(CartItemAddModel cartItem);
    Task RemoveAsync(long cartItemId);
    Task<CartItemModel> UpdateAsync(CartItemUpdateModel cartItem);
}
