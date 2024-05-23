using BookShop.Services.Models.BillingModels;

namespace BookShop.Services.Models.OrderModels;

public class OrderModelWithPaymentResult
{
    public OrderModel Order { get; set; }
    public PaymentResult PaymentResult { get; set; }
    public long PaymentMethodId { get; set; }
}
