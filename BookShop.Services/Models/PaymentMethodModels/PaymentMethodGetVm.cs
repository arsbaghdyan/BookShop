using BookShop.Data.Enums;

namespace BookShop.Services.Models.CartItemModels;

public class PaymentMethodGetVm
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Details { get; set; }
}
