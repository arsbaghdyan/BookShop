using BookShop.Data.Abstractions;
using BookShop.Data.Enums;

namespace BookShop.Data.Entities;

public class PaymentMethodEntity : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Details { get; set; }
    public ClientEntity? ClientEntity { get; set; }
    public List<PaymentEntity>? Payments { get; set; }
    public List<OrderEntity>? OrderEntities { get; set; }
}