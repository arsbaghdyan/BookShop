using BookShop.Data.Abstractions;

namespace BookShop.Api.Models.ProductModels;

public class ProductRemoveModel : IIdentifiable
{
    public long Id { get; set; }
}
