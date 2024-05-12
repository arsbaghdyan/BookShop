using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface ICartService
{
    Task CreateAsync(long clientId);
    Task<List<CartItemModel>> GetAllCartItemsAsync(long cartId);
    Task ClearAsync(long cartId);
}
