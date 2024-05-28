namespace BookShop.Data.Models;

public class CardDetails
{
    public long CardNumber { get; set; }
    public int CVV { get; set; }
    public DateTime ExpiredAt { get; set; }
    public string FullName { get; set; } = null!;
}