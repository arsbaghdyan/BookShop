namespace BookShop.Services.Models.OrderModels;

public class OrderAddFromCartModel
{
    public long CartItemId { get; set; }
    public long PaymentMethodId { get; set; }
}
