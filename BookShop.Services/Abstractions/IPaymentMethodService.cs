using BookShop.Data.Models;
using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IPaymentMethodService
{
    Task<List<PaymentMethodModel>> GetAllAsync();
    Task<PaymentMethodModel> AddCardAsync(CardDetails cardDetails);
    Task RemoveAsync(long paymentMethodId);
}
