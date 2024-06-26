﻿using BookShop.Services.Models.PaymentModels;

namespace BookShop.Services.Abstractions;

public interface IPaymentService
{
    Task<PaymentModel?> GetByIdAsync(long paymentId);
    Task<PaymentModel> PayAsync(long invoiceId);
}
