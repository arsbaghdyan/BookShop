namespace BookShop.Services.Models.OrderModels;

public class OrderAddModel
{
    public List<OrderItemModel> OrderItems { get; set; }
    public long PaymentMethodId { get; set; }
}
