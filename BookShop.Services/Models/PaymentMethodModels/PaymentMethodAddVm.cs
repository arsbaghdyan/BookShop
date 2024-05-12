using BookShop.Data.Enums;

namespace BookShop.Services.Models.CartItemModels;


public class PaymentMethodAddModel
{
    public long ClientId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Details { get; set; }
}
