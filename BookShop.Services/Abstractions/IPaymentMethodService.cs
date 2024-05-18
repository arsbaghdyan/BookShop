using BookShop.Data.Models;
using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IPaymentMethodService
{
    Task<PaymentMethodModel> AddCardAsync(CardDetails cardDetails);
    Task RemoveAsync(long paymentMethodId);
    Task<List<PaymentMethodModel>> GetAllAsync();
}
