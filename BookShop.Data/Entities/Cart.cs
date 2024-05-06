using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class Cart : IIdentifiable
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public Dictionary<int, int> ProductId_Count { get; set; } = new();
    public decimal Price { get; set; }
    public Client? Client { get; set; }
    public Product? Product { get; set; }
}