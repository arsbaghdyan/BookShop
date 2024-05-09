using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class ProductEntity : IIdentifiable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Manufacturer { get; set; }
    public string Details { get; set; }
    public int Count { get; set; }
    public List<OrderEntity> Orders { get; set; } = new();
    public WishListItemEntity? WishListItemEntity { get; set; }
    public CartItemEntity? CartItemEntity { get; set; }
}