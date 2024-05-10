using BookShop.Data.Abstractions;

namespace BookShop.Api.Models.WishListItemModels;

public class WishListItemDeleteModel : IIdentifiable
{
    public long Id { get; set; }
}
