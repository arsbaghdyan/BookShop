using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class Cart:IIdentifiable
{
    public long Id { get; set; }
    public List<CartItem> CartItems { get; set; } = new();
}
