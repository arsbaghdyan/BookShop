﻿using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Data.Enums;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.PaymentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class PaymentService : IPaymentService
{
    private readonly IClientContextReader _clientContextReader;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;
    private readonly BookShopDbContext _bookShopDbContext;

    public PaymentService(IClientContextReader clientContextReader, IMapper mapper,
                          ILogger<PaymentService> logger, BookShopDbContext bookShopDbContext)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _logger = logger;
        _bookShopDbContext = bookShopDbContext;
    }

    public async Task<PaymentModel> ConfirmPayment(PaymentAddModel paymentAddModel)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var invoiceEntity = await _bookShopDbContext.Invoices.Where(i => i.ClientId == clientId).FirstOrDefaultAsync(i => i.Id == paymentAddModel.InvoiceId);

        if (invoiceEntity == null)
        {
            throw new Exception("One of your input parametr is invalid");
        }

        var paymentEntity = new PaymentEntity();

        paymentEntity.PaymentMethodId = paymentAddModel.PaymentMethodId;
        paymentEntity.Amount = paymentAddModel.Amount;
        paymentEntity.InvoiceId = invoiceEntity.Id;

        if (invoiceEntity.TotalAmount == paymentEntity.Amount)
        {
            paymentEntity.PaymentStatus = PaymentStatus.Success;
            _logger.LogInformation($"Payment with Id {paymentEntity.Id} is success for client with Id {clientId}");
        }
        else
        {
            paymentEntity.PaymentStatus = PaymentStatus.Fail;
            _logger.LogInformation($"Payment with Id {paymentEntity.Id} is fail for client with Id {clientId}");
        }

        _bookShopDbContext.Payments.Add(paymentEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Payment with Id {paymentEntity.Id} is added for client with Id {clientId}");

        var paymentModel = _mapper.Map<PaymentModel>(paymentEntity);

        return paymentModel;
    }
}
