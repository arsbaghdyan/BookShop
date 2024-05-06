using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class Invoice : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long PaymentId { get; set; }
    public long OrderId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsPaid { get; set; }
    public Payment? Payment { get; set; }
    public Order? Order { get; set; }
    public Client? Client { get; set; }
}