using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class ProductEntity : IIdentifiable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Manufacturer { get; set; }
    public int Count { get; set; }
    public List<OrderEntity> Orders { get; set; } = new();
    public List<WishListItemEntity> WishListItemEntity { get; set; } = new();
    public List<CartItemEntity> CartItemEntity { get; set; } = new();
}