using BookShop.Data.Models;
using BookShop.Services.Models.PaymentMethodModels;

namespace BookShop.Services.Abstractions;

public interface IPaymentMethodService
{
    Task<List<BankCardInfo?>> GetAllAsync();
    Task<BankCardInfo?> AddCardAsync(CardDetails cardDetails);
    Task RemoveAsync(long paymentMethodId);
}
