using BookShop.Data.Abstractions;

namespace BookShop.Api.Models.ProductModels;

public class ProductGetModel : IIdentifiable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Manufacturer { get; set; }
    public string Details { get; set; }
    public int Count { get; set; }
}
