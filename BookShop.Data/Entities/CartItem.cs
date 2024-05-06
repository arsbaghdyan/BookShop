using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class CartItem : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long ProductId { get; set; }
    public long CartId { get; set; }
    public long Count { get; set; }
    public decimal Price { get; set; }
    public Client? Client { get; set; }
    public Product? Product { get; set; }
    public Cart? Cart { get; set; }
}