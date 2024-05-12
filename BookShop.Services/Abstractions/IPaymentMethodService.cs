using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IPaymentMethodService
{
    Task AddAsync(PaymentMethodAddModel paymentMethodEntity);
    Task RemoveAsync(long paymentMethodId);
    Task<List<PaymentMethodModel>> GetAllAsync(long clientId);
}
