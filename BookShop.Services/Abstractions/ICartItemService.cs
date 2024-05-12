﻿using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface ICartItemService
{
    Task AddAsync(CartItemAddModel cartItem);
    Task RemoveAsync(long cartId);
    Task UpdateAsync(CartItemUpdateModel cartItem);
}
