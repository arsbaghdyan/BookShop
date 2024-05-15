using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Data.Models;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookShop.Services.Impl;

internal class PaymentMethodService : IPaymentMethodService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<PaymentMethodService> _logger;
    private readonly IClientContextReader _clientContextReader;

    public PaymentMethodService(BookShopDbContext bookShopDbContext, ILogger<PaymentMethodService> logger, IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _clientContextReader = clientContextReader;
    }

    public async Task<PaymentMethodModel> AddAsync(PaymentMethodAddModel paymentMethodAddModel)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var paymentMethod = new PaymentMethodEntity
        {
            ClientId = clientId,
            PaymentMethod = paymentMethodAddModel.PaymentMethod,
            Details = JsonConvert.SerializeObject(paymentMethodAddModel.Details)
        };

        _bookShopDbContext.PaymentMethods.Add(paymentMethod);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"PaymentMethod with Id {paymentMethod.Id} added successfully for client with id {clientId}.");

        var paymentMethodModel = new PaymentMethodModel
        {
            Id = paymentMethod.Id,
            PaymentMethod = paymentMethodAddModel.PaymentMethod,
            Details = JsonConvert.DeserializeObject<CardDetails>(paymentMethod.Details)
        };

        return paymentMethodModel;
    }

    public async Task<List<PaymentMethodModel>> GetAllAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var paymentMethodsDb = await _bookShopDbContext.PaymentMethods.Where(pm => pm.ClientId == clientId).ToListAsync();

        var paymentMethods = new List<PaymentMethodModel>();

        foreach (var paymentMethod in paymentMethodsDb)
        {
            var paymentMethodModel = new PaymentMethodModel
            {
                Id = paymentMethod.Id,
                PaymentMethod = paymentMethod.PaymentMethod,
                Details = JsonConvert.DeserializeObject<CardDetails>(paymentMethod.Details)
            };
            paymentMethods.Add(paymentMethodModel);
        }

        return paymentMethods;
    }

    public async Task RemoveAsync(long paymentMethodId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var paymentMethod = await _bookShopDbContext.PaymentMethods.FirstOrDefaultAsync(p => p.Id == paymentMethodId && p.ClientId == clientId);

        _bookShopDbContext.PaymentMethods.Remove(paymentMethod);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"PaymentMethod with Id {paymentMethod.Id} removed successfully for client with id {clientId}.");
    }
}
