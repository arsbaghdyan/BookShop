﻿namespace BookShop.Services.Models.CartItemModels;

public class CartItemAddVm
{
    public long ProductId { get; set; }
    public long CartId { get; set; }
    public long Count { get; set; }
    public decimal Price { get; set; }
}
