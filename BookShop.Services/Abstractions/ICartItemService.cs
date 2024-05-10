using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface ICartItemService
{
    Task AddAsync(CartItemEntity cartItem);
    Task RemoveAsync(CartItemEntity cartItem);
    Task UpdateAsync(CartItemEntity cartItemEntity);
}
