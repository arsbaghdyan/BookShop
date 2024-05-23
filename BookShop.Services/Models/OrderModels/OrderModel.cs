namespace BookShop.Services.Models.OrderModels;

public class OrderModel
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public decimal Amount { get; set; }
    public int Count { get; set; }
}
