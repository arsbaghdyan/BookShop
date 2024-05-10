using BookShop.Data.Abstractions;

namespace BookShop.Api.Models.CartItemModels;

public class CartItemDeleteModel : IIdentifiable
{
    public long Id { get; set; }
}
