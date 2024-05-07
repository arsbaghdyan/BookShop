using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class WishList : IIdentifiable
{
    public long Id { get; set; }
    public List<WishListItem> WishListItems { get; set; } = new();
}
