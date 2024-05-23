using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Data.Enums;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.BillingModels;
using BookShop.Services.Models.OrderModels;
using BookShop.Services.Models.PaymentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            .Include(p => p.InvoiceEntity)
            .FirstOrDefaultAsync(p => p.Id == paymentId && p.InvoiceEntity.ClientId == clientId);

        return _mapper.Map<PaymentModel?>(paymentEntity);
    }

    public async Task<PaymentModel> PayAsync(long invoiceId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var invoiceEntity = await _bookShopDbContext.Invoices
           .FirstOrDefaultAsync(p => p.Id == invoiceId && p.ClientId == clientId);

        var paymentEntity = new PaymentEntity
        {
            Amount = invoiceEntity.TotalAmount,
            InvoiceEntity = invoiceEntity
        };

        _bookShopDbContext.Payments.Add(paymentEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Payment with Id {paymentEntity.Id} is added for '{clientId}' client.");

        return _mapper.Map<PaymentModel>(paymentEntity);
    }
}
