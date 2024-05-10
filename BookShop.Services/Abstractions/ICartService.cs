using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface ICartService
{
    Task CreateAsync(long clientId);
    Task<List<CartItemEntity>> GetAllCartItemsAsync(long cartId);
    Task ClearAsync(long cartId);
}
