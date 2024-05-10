using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface ICartService
{
    Task CreateAsync(long clientId);
    Task<List<CartItemEntity>> GetAllCartItems(long cartId);
    Task AddItemAsync(CartItemEntity cartItem);
    Task RemoveItemAsync(CartItemEntity cartItem);
}
