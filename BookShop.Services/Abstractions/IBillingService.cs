using BookShop.Services.Models.BillingModels;

namespace BookShop.Services.Abstractions;

public interface IBillingService
{
    Task<PaymentResponse> PayViaCardAsync(PaymentRequest<BankCardInfo> paymentRequest); 
}
