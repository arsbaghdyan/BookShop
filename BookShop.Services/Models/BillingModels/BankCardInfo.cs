namespace BookShop.Services.Models.BillingModels;

public class BankCardInfo
{
    public long CardNumber { get; set; }
    public int CVV { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public string FullName { get; set; } = null!;
}