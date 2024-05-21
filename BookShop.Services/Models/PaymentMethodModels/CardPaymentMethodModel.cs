using BookShop.Data.Enums;
using BookShop.Data.Models;
using BookShop.Services.Models.BillingModels;

namespace BookShop.Services.Models.CartItemModels;

public class CardPaymentMethodModel
{
    public long Id { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public CardDetails? Details { get; set; }
}
