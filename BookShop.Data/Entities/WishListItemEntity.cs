using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class WishListItemEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long WishListId { get; set; }
    public ClientEntity? ClientEntity { get; set; }
    public ProductEntity? ProductEntity { get; set; }
    public WishListEntity? WishListEntity { get; set; }
}