﻿namespace BookShop.Services.Models.CartItemModels;

public class WishListItemModel
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long WishListId { get; set; }
}
