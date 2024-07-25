using BookShop.Services.Models.ProductModels;

namespace BookShop.Services.Models.OrderProductModels;

public class OrderProductModel
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public ProductModel? Product { get; set; }
}
