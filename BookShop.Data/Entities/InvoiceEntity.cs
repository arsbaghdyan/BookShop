using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class InvoiceEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderEntity? OrderEntity { get; set; }
    public ClientEntity? ClientEntity { get; set; }
}