using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class PaymentMethod : IIdentifiable
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string WayToPay { get; set; }
    public string Details { get; set; }
    public Client? Client { get; set; }
    public List<Payment>? Payments { get; set; }
}