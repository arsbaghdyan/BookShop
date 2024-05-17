using BookShop.Services.Models.PaymentModels;

namespace BookShop.Services.Abstractions;

public interface IPaymentService
{
    Task<PaymentModel> ApprovePayment(PaymentAddModel paymentAddModel);
}
