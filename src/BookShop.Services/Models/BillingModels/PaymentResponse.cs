namespace BookShop.Services.Models.BillingModels;

public class PaymentResponse
{
    public decimal Amount { get; set; }
    public PaymentResult Result { get; set; }
}
