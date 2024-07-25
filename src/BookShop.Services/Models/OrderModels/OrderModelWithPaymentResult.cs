using BookShop.Data.Enums;

namespace BookShop.Services.Models.OrderModels;

public class OrderModelWithPaymentResult
{
    public OrderModel Order { get; set; }
    public PaymentStatus PaymentResult { get; set; }
    public long PaymentMethodId { get; set; }
}
