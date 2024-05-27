using BookShop.Services.Models.OrderProductModels;

namespace BookShop.Services.Models.OrderModels;

public class OrderModel
{
    public long Id { get; set; }
    public List<OrderProductModel> OrderProducts { get; set; } = new();
    public decimal Amount { get; set; }
    public int Count { get; set; }
}
