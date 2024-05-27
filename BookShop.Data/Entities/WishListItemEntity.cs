using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class WishListItemEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long WishListId { get; set; }
    public ProductEntity? Product { get; set; }
    public WishListEntity? WishList { get; set; }
}