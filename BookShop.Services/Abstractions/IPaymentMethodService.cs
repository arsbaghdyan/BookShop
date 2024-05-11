using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface IPaymentMethodService
{
    Task AddAsync(PaymentMethodEntity paymentMethodEntity);
    Task RemoveAsync(PaymentMethodEntity paymentMethodEntity);
    Task<List<PaymentMethodEntity>> GetAllAsync(long clientId);
}
