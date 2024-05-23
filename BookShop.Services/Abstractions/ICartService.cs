using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface ICartService
{
    Task<List<CartItemModel?>> GetAllCartItemsAsync();
    Task<CartItemModel?> AddAsync(CartItemAddModel cartItem);
    Task<CartItemModel?> UpdateAsync(CartItemUpdateModel cartItemUpdateModel);
    Task RemoveAsync(long productId);
    Task ClearAsync();
}
