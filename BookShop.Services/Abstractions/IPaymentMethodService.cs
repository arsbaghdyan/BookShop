using BookShop.Data.Models;
using BookShop.Services.Models.BillingModels;
using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IPaymentMethodService
{
    Task<List<CardPaymentMethodModel>> GetAllAsync();
    Task<CardPaymentMethodModel> AddCardAsync(CardDetails cardDetails);
    Task RemoveAsync(long paymentMethodId);
}
