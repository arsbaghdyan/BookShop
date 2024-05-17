using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class OrderEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long ProductId { get; set; }
    public long Count { get; set; }
    public decimal Amount { get; set; }
    public InvoiceEntity? InvoiceEntity { get; set; }
    public ProductEntity? ProductEntity { get; set; }
    public ClientEntity? ClientEntity { get; set; }
}