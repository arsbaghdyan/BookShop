﻿using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Data.Enums;
using BookShop.Data.Models;
using BookShop.Services.Abstractions;
using BookShop.Services.Extensions;
using BookShop.Services.Models.BillingModels;
using BookShop.Services.Models.PaymentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookShop.Services.Impl;

internal class PaymentService : IPaymentService
{
    private readonly IClientContextReader _clientContextReader;
    private readonly IMapper _mapper;
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly IBillingService _billingService;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IClientContextReader clientContextReader,
                          IMapper mapper,
                          ILogger<PaymentService> logger,
                          BookShopDbContext bookShopDbContext,
                          IBillingService billingService)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _bookShopDbContext = bookShopDbContext;
        _billingService = billingService;
        _logger = logger;
    }

    public async Task<PaymentModel?> GetByIdAsync(long paymentId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var paymentEntity = await _bookShopDbContext.Payments
            .Include(p => p.Invoice)
            .FirstOrDefaultAsync(p => p.Id == paymentId && p.Invoice.ClientId == clientId);

        return _mapper.Map<PaymentModel?>(paymentEntity);
    }

    public async Task<PaymentModel> PayAsync(long invoiceId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var invoiceEntity = await _bookShopDbContext.Invoices
            .Include(i => i.Order)
            .ThenInclude(o => o.PaymentMethod)
            .FirstOrDefaultAsync(p => p.Id == invoiceId && p.ClientId == clientId);

        var paymentMethodDetails = invoiceEntity.Order.PaymentMethod.Details;

        var bankCard = JsonConvert.DeserializeObject<CardDetails>(paymentMethodDetails);

        var paymentRequest = new PaymentRequest<BankCardInformation>()
        {
            Amount = invoiceEntity.TotalAmount,
            PaymentMethod = _mapper.Map<BankCardInformation>(bankCard)
        };

        var paymentResponse = await _billingService.PayViaCardAsync(paymentRequest);

        var paymentEntity = new PaymentEntity
        {
            Amount = invoiceEntity.TotalAmount,
            Invoice = invoiceEntity,
            PaymentMethodId = invoiceEntity.Order.PaymentMethod.Id,
            PaymentStatus = paymentResponse.GetPaymentStatus()
        };

        if (paymentEntity.Amount == invoiceEntity.TotalAmount && paymentEntity.PaymentStatus == PaymentStatus.Success)
        {
            paymentEntity.Invoice.InvoiceStatus = InvoiceStatus.Payed;
        }
        else
        {
            paymentEntity.Invoice.InvoiceStatus = InvoiceStatus.Declined;
        }

        _bookShopDbContext.Payments.Add(paymentEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Payment with Id {paymentEntity.Id} is added for '{clientId}' client.");

        return _mapper.Map<PaymentModel>(paymentEntity);
    }
}
