using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class WishList : IIdentifiable
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public Client? Client { get; set; }
    public Product? Product { get; set; }
}