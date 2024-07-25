using BookShop.Data.Enums;

namespace BookShop.Services.Models.PaymentModels;

public class PaymentModel
{
    public long Id { get; set; }
    public long PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}
