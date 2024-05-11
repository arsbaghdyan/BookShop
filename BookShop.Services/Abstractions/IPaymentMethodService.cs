using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IPaymentMethodService
{
    Task AddAsync(PaymentMethodAddVm paymentMethodEntity);
    Task RemoveAsync(long paymentMethodId);
    Task<List<PaymentMethodGetVm>> GetAllAsync(long clientId);
}
