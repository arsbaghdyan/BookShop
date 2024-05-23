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
    private readonly BookShopDbContext _bookShopDbContext;

    public PaymentService(IClientContextReader clientContextReader,
                          IMapper mapper,
                          ILogger<PaymentService> logger,
                          BookShopDbContext bookShopDbContext)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _bookShopDbContext = bookShopDbContext;
    }

    public async Task<PaymentModel?> GetByIdAsync(long paymentId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var paymentEntity = await _bookShopDbContext.Payments
            .Where(p => p.InvoiceEntity.ClientId == clientId)
            .FirstOrDefaultAsync(p => p.PaymentMethodId == paymentId);

        return _mapper.Map<PaymentModel?>(paymentEntity);
    }
}
