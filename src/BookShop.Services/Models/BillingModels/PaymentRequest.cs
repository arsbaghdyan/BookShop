namespace BookShop.Services.Models.BillingModels;

public class PaymentRequest<TPaymentMethod>
{
    public decimal Amount { get; set; }
    public TPaymentMethod PaymentMethod { get; set; } = default!;
}
