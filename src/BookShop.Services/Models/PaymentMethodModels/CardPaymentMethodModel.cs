using BookShop.Data.Enums;
using BookShop.Data.Models;

namespace BookShop.Services.Models.PaymentMethodModels;

public class BankCardInfo
{
    public long Id { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public CardDetails? Details { get; set; }
}
