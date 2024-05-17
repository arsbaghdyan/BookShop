namespace BookShop.Services.Models.PaymentModels;

public class PaymentAddModel
{
    public long PaymentMethodId { get; set; }
    public long InvoiceId { get; set; }
    public decimal Amount { get; set; }
}
