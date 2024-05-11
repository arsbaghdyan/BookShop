using BookShop.Data.Abstractions;
using BookShop.Data.Enums;

namespace BookShop.Api.Models.PaymentMethodModels;

public class PaymentMethodGetModel : IIdentifiable
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Details { get; set; }
}
