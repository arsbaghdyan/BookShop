using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class ProductEntity : IIdentifiable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Manufacturer { get; set; }
    public int Count { get; set; }
    public List<WishListItemEntity> WishListItems { get; set; } = new();
    public List<CartItemEntity> CartItems { get; set; } = new();
    public List<OrderProduct> OrderProducts { get; set; } = new();
}