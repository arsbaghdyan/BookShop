using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class CardDetails : IIdentifiable
{
    public long Id { get; set; }
    public long CardNumber { get; set; }
    public int CVV { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public string FullName { get; set; } = null!;
}
