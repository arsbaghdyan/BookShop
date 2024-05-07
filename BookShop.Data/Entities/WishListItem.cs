using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class WishListItem : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long ProductId { get; set; }
    public long WishListId { get; set; }
    public Client? Client { get; set; }
    public Product? Product { get; set; }
    public WishList? WishList { get; set; }
}