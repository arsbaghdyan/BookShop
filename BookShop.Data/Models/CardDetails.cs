namespace BookShop.Data.Models;

public class CardDetails
{
    public long CardNumber { get; set; }
    public int CVV { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public string FullName { get; set; } = null!;
}