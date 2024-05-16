using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Data.Enums;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.PaymentModels;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class PaymentService : IPaymentService
{
    private readonly IClientContextReader _clientContextReader;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    private readonly BookShopDbContext _bookShopDbContext;

    public PaymentService(IClientContextReader clientContextReader, IMapper mapper, ILogger<OrderService> logger, BookShopDbContext bookShopDbContext)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _logger = logger;
        _bookShopDbContext = bookShopDbContext;
    }

    public async Task<PaymentModel> ApprovePayment(PaymentAddModel paymentAddModel)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var payment = _bookShopDbContext.Payments.FirstOrDefault(p => p.Id == paymentAddModel.PaymentMethodId);

        payment.PaymentStatus = PaymentStatus.Success;

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Payment with Id {payment.Id} is success for client with Id {clientId}");

        var invoice = new InvoiceEntity
        {
            ClientId = clientId,
            PaymentId = payment.Id,
            OrderId = payment.InvoiceEntity.OrderId, 
            CreatedAt = DateTime.UtcNow,
            TotalAmount = payment.Amount, 
            IsPaid = true, 
            PaymentEntity = payment
        };

        _bookShopDbContext.Invoices.Add(invoice);

        var paymentModel = _mapper.Map<PaymentModel>(payment);

        return paymentModel;
    }

    public async Task<PaymentModel> CancelPayment(long paymentId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var payment = _bookShopDbContext.Payments.FirstOrDefault(p => p.Id == paymentId);

        payment.PaymentStatus = PaymentStatus.Denied;

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Payment with Id {payment.Id} is denied for client with Id {clientId}");

        var invoice = new InvoiceEntity
        {
            ClientId = clientId,
            PaymentId = payment.Id,
            OrderId = payment.InvoiceEntity.OrderId,
            CreatedAt = DateTime.UtcNow,
            TotalAmount = payment.Amount,
            IsPaid = false,
            PaymentEntity = payment
        };

        _bookShopDbContext.Invoices.Add(invoice);

        var paymentModel = _mapper.Map<PaymentModel>(payment);

        return paymentModel;
    }
}
