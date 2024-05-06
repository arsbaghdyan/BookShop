using BookShop.Data.Abstractions;
using BookShop.Data.Enums;

namespace BookShop.Data.Entities;

public class Payment : IIdentifiable
{
    public long Id { get; set; }
    public long PaymentMethodId { get; set; }
    public decimal Price { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
}