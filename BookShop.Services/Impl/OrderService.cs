using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.OrderModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class OrderService : IOrderService
{
    private readonly IClientContextReader _clientContextReader;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    private readonly BookShopDbContext _bookShopDbContext;

    public OrderService(IClientContextReader clientContextReader, IMapper mapper, ILogger<OrderService> logger, BookShopDbContext bookShopDbContext)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _logger = logger;
        _bookShopDbContext = bookShopDbContext;
    }

    public async Task<OrderModel> AddOrderAsync(OrderAddModel orderAddModel)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var order = await _bookShopDbContext.Orders.FirstOrDefaultAsync(o => o.ClientId == clientId);

        var product = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == orderAddModel.ProductId);

        //var orderToAdd = _mapper.Map<OrderModel>();

        return default;
    }

    public Task ClearAsync()
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(long orderId)
    {
        throw new NotImplementedException();
    }
}
