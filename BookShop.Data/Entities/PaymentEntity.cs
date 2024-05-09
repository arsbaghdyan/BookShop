using BookShop.Data.Abstractions;
using BookShop.Data.Enums;

namespace BookShop.Data.Entities;

public class PaymentEntity : IIdentifiable
{
    public long Id { get; set; }
    public long PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentMethodEntity? PaymentMethodEntity { get; set; }
    public InvoiceEntity? InvoiceEntity { get; set; }
}