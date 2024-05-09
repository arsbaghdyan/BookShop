using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class WishListEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public List<WishListItemEntity> WishListItems { get; set; } = new();
    public ClientEntity? ClientEntity { get; set; }
}