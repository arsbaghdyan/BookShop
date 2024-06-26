﻿using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Data.Enums;
using BookShop.Data.Models;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.PaymentMethodModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookShop.Services.Impl;

internal class PaymentMethodService : IPaymentMethodService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<PaymentMethodService> _logger;
    private readonly IClientContextReader _clientContextReader;

    public PaymentMethodService(BookShopDbContext bookShopDbContext,
                                ILogger<PaymentMethodService> logger,
                                IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _clientContextReader = clientContextReader;
    }
    public async Task<List<BankCardInfo?>> GetAllAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var paymentMethodEntites = await _bookShopDbContext.PaymentMethods
            .Where(p => p.ClientId == clientId).ToListAsync();

        var paymentMethodModels = new List<BankCardInfo?>();

        foreach (var paymentMethod in paymentMethodEntites)
        {
            var paymentMethodModel = new BankCardInfo
            {
                Id = paymentMethod.Id,
                PaymentMethod = paymentMethod.PaymentMethod,
                Details = JsonConvert.DeserializeObject<CardDetails>(paymentMethod.Details)
            };
            paymentMethodModels.Add(paymentMethodModel);
        }

        return paymentMethodModels;
    }

    public async Task<BankCardInfo?> AddCardAsync(CardDetails cardDetails)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var paymentMethodEntity = new PaymentMethodEntity
        {
            ClientId = clientId,
            PaymentMethod = PaymentMethod.Card,
            Details = JsonConvert.SerializeObject(cardDetails)
        };

        _bookShopDbContext.PaymentMethods.Add(paymentMethodEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"PaymentMethod with Id {paymentMethodEntity.Id} added successfully for '{clientId}' client.");

        var paymentMethodModel = new BankCardInfo
        {
            Id = paymentMethodEntity.Id,
            PaymentMethod = PaymentMethod.Card,
            Details = cardDetails
        };

        return paymentMethodModel;
    }

    public async Task RemoveAsync(long paymentMethodId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        await _bookShopDbContext.PaymentMethods
           .Where(c => c.ClientId == clientId && c.Id == paymentMethodId)
           .ExecuteDeleteAsync();

        _logger.LogInformation($"PaymentMethod with {paymentMethodId} Id removed successfully for '{clientId}' client.");
    }
}
