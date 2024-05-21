using BookShop.Services.Abstractions;
using BookShop.Services.Models.BillingModels;

namespace BookShop.Services.Mock;

internal class MockBillingService : IBillingService
{
    public Task<PaymentResponse> PayViaCardAsync(PaymentRequest<BankCardInfo> paymentRequest)
    {
        var response = new PaymentResponse
        {
            Amount = paymentRequest.Amount,
            Result = PaymentResult.Success
        };

        return Task.FromResult(response);
    }
}
