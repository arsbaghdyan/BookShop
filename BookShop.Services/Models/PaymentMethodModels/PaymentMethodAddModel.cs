using BookShop.Data.Enums;
using BookShop.Data.Models;

namespace BookShop.Services.Models.CartItemModels;


public class PaymentMethodAddModel
{
    public PaymentMethod PaymentMethod { get; set; }
    public CardDetails? Details { get; set; }
}
