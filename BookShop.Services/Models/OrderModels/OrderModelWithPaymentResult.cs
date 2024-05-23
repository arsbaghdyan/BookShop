using BookShop.Services.Models.BillingModels;

namespace BookShop.Services.Models.OrderModels;

public class OrderModelWithPaymentResult
{
    public long ProductId { get; set; }
    public decimal Amount { get; set; }
    public int Count { get; set; }
    public PaymentResult PaymentResult { get; set; }
    public long PaymentMethodId { get; set; }
}
