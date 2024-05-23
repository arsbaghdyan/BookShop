using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
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

        return _mapper.Map<InvoiceModel?>(invoiceEntity);
    }
}
