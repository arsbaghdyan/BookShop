using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.InvoiceModels;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Impl;

internal class InvoiceService : IInvoiceService
{
    private readonly IClientContextReader _clientContextReader;
    private readonly IMapper _mapper;
    private readonly BookShopDbContext _bookShopDbContext;

    public InvoiceService(IClientContextReader clientContextReader,
                          IMapper mapper,
                          BookShopDbContext bookShopDbContext)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _bookShopDbContext = bookShopDbContext;
    }

    public async Task<InvoiceModel?> GetByIdAsync(long invoiceId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var invoiceEntity = await _bookShopDbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == invoiceId && i.ClientId == clientId);

        if (invoiceEntity == null)
        {
            throw new Exception($"Invoice with Id {invoiceId} not found for client '{clientId}'.");
        }

        return _mapper.Map<InvoiceModel?>(invoiceEntity);
    }

    public async Task<InvoiceModel> CreateInvoiceAsync(OrderEntity orderEntity)
    {
        var clientId = _clientContextReader.GetClientContextId();

        if (orderEntity == null)
        {
            throw new Exception($"Order with id {orderEntity.Id} not found for client '{clientId}'.");
        }

        if (orderEntity.ClientId != clientId)
        {
            throw new InvalidOperationException($"Order does not belong to client '{clientId}'.");
        }

        var invoice = new InvoiceEntity
        {
            ClientId = clientId,
            CreatedAt = DateTime.UtcNow,
            Order = orderEntity,
            TotalAmount = orderEntity.Amount,
        };

        _bookShopDbContext.Invoices.Add(invoice);
        await _bookShopDbContext.SaveChangesAsync();

        return _mapper.Map<InvoiceModel>(invoice);
    }
}
