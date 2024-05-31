namespace BookShop.Services.Models.OrderModels;

public class OrderAddFromCartModel
{
    public List<long> CartItemIds { get; set; }
    public long PaymentMethodId { get; set; }
}
