using BookShop.Data.Abstractions;

namespace BookShop.Api.Models.PaymentMethodModels;

public class PaymentMethodDeleteModel : IIdentifiable
{
    public long Id { get; set; }
}
