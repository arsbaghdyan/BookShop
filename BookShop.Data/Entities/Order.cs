using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class Order : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long ProductId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public Product? Product { get; set; }
    public Client? Client { get; set; }
}