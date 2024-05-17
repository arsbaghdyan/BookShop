using AutoMapper;
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

    public async Task<PaymentModel> ApprovePayment(PaymentAddModel paymentAddModel)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var invoice = await _bookShopDbContext.Invoices.Where(i => i.ClientId == clientId).FirstOrDefaultAsync(i => i.ClientId == clientId);

        var payment = new PaymentEntity();

        payment.PaymentMethodId = paymentAddModel.PaymentMethodId;
        payment.Amount = paymentAddModel.Amount;
        payment.InvoiceId = invoice.Id;

        if (invoice.TotalAmount == payment.Amount)
        {
            payment.PaymentStatus = PaymentStatus.Success;
        }
        else
        {
            payment.PaymentStatus = PaymentStatus.Fail;
        }

        _bookShopDbContext.Payments.Add(payment);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Payment with Id {payment.Id} is success for client with Id {clientId}");

        var paymentModel = _mapper.Map<PaymentModel>(payment);

        return paymentModel;
    }
}
