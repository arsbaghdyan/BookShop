using BookShop.Data.Abstractions;
using BookShop.Data.Enums;

namespace BookShop.Data.Entities;

public class InvoiceEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public InvoiceStatus InvoiceStatus { get; set; }
    public OrderEntity? Order { get; set; }
    public ClientEntity? Client { get; set; }
    public List<PaymentEntity> Payments { get; set; } = new List<PaymentEntity>();
}