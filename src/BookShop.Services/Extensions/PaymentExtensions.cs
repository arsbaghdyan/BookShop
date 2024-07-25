using BookShop.Data.Enums;
using BookShop.Services.Models.BillingModels;

namespace BookShop.Services.Extensions;

public static class PaymentExtensions
{
    public static PaymentStatus GetPaymentStatus(this PaymentResponse response)
        => response.Result switch
        {
            PaymentResult.Success => PaymentStatus.Success,
            PaymentResult.Failed => PaymentStatus.Failed,
            _ => throw new ArgumentOutOfRangeException(nameof(response.Result)),
        };
}
