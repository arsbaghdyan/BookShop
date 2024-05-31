using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class CartItemEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long CartId { get; set; }
    public int Count { get; set; }
    public ProductEntity? Product { get; set; }
    public CartEntity? Cart { get; set; }
}