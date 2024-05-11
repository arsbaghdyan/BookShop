using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface ICartItemService
{
    Task AddAsync(CartItemAddVm cartItem);
    Task RemoveAsync(long cartId);
    Task UpdateAsync(CartItemUpdateVm cartItem);
}
