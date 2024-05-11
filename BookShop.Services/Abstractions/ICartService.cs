using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface ICartService
{
    Task CreateAsync(long clientId);
    Task<List<CartItemGetVm>> GetAllCartItemsAsync(long cartId);
    Task ClearAsync(long cartId);
}
