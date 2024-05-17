using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.InvoiceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class InvoiceService : IInvoiceService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<WishListService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public InvoiceService(BookShopDbContext bookShopDbContext, ILogger<WishListService> logger,
                          IMapper mapper, IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var invoiceEntity = await _bookShopDbContext.Invoices.Where(i => i.ClientId == clientId).ToListAsync();

        _bookShopDbContext.Invoices.RemoveRange(invoiceEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Invoices cleared successfully for client with id {clientId}.");
    }

    public async Task<List<InvoiceModel>> GetAllAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var invoiceEntities = await _bookShopDbContext.Invoices.Where(i => i.ClientId == clientId).ToListAsync();

        var invoiceModel = _mapper.Map<List<InvoiceModel>>(invoiceEntities);

        return invoiceModel;
    }

    public async Task<InvoiceModel> GetByIdAsync(long invoiceId)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var invoiceEntity = await _bookShopDbContext.Invoices.Where(i => i.ClientId == clientId).FirstOrDefaultAsync(i => i.Id == invoiceId);

        var invoiceModel = _mapper.Map<InvoiceModel>(invoiceEntity);

        return invoiceModel;
    }
}
