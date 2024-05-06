using BookShop.Data.Abstractions;
using BookShop.Data.Enums;

namespace BookShop.Data.Entities;

public class PaymentMethod : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public PayingMethod PayingMethod { get; set; }
    public CardDetails Details { get; set; }
    public Client? Client { get; set; }
    public List<Payment>? Payments { get; set; }
}