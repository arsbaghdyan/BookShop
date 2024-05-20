using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.OrderModel;
using BookShop.Services.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class OrderService : IOrderService
{
    private readonly IClientContextReader _clientContextReader;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    private readonly BookShopDbContext _bookShopDbContext;

    private record OrderInfo(long ProductId, int Count);

    public OrderService(IClientContextReader clientContextReader, IMapper mapper,
                        ILogger<OrderService> logger, BookShopDbContext bookShopDbContext)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _logger = logger;
        _bookShopDbContext = bookShopDbContext;
    }

    public async Task<OrderModel> AddOrderAsync(OrderAddModel orderAddModel)
    {
        var orderInfo = new OrderInfo(orderAddModel.ProductId, orderAddModel.Count);
        return await PlaceOrderInternalAsync(orderInfo);
    }

    public async Task<OrderModel> AddOrderFromCartAsync(OrderAddFromCardModel orderAddFromCardModel)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cartItemEntity = await _bookShopDbContext.CartItems
            .Include(c => c.ProductEntity)
            .FirstOrDefaultAsync(c => c.Id == orderAddFromCardModel.CartItemId &&
                c.CartEntity.ClientId == clientId);

        if (cartItemEntity == null)
        {
            throw new Exception($"CartItem with Id {orderAddFromCardModel.CartItemId} not found for client with Id {clientId}");
        }

        var orderInfo = new OrderInfo(cartItemEntity.ProductId, cartItemEntity.Count);
        var order = await PlaceOrderInternalAsync(orderInfo);

        _bookShopDbContext.CartItems.Remove(cartItemEntity);
        await _bookShopDbContext.SaveChangesAsync();

        return order;
    }

    private async Task<OrderModel> PlaceOrderInternalAsync(OrderInfo productInfo)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderEntity = await _bookShopDbContext.Orders
            .FirstOrDefaultAsync(o => o.ClientId == clientId && o.ProductId == productInfo.ProductId);
        var productEntity = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == productInfo.ProductId);

        if (orderEntity == null || productEntity == null)
        {
            throw new Exception($"Input parametr for productId {productInfo.ProductId} is invalid");
        }

        var orderModel = new OrderModel();
        if (orderEntity != null)
        {
            orderEntity.Count += productInfo.Count;
            orderEntity.Amount = productEntity.Price * orderEntity.Count;

            await _bookShopDbContext.SaveChangesAsync();

            _logger.LogInformation($"Order with Id{orderEntity.Id} added successefully for client with id {clientId}");

            orderModel = _mapper.Map<OrderModel>(orderEntity);
        }

        var orderToAdd = _mapper.Map<OrderEntity>(productInfo);

        orderToAdd.Amount = productEntity.Price * orderToAdd.Count;
        orderToAdd.ClientId = clientId;

        _bookShopDbContext.Orders.Add(orderToAdd);

        var invoice = new InvoiceEntity
        {
            ClientId = clientId,
            CreatedAt = DateTime.UtcNow,
            OrderEntity = orderToAdd,
            TotalAmount = orderToAdd.Amount,
        };

        _bookShopDbContext.Invoices.Add(invoice);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Order with Id{orderToAdd.Id} added successefully for client with id {clientId}");

        return _mapper.Map<OrderModel>(orderToAdd);
    }

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        await _bookShopDbContext.Orders
           .Where(c => c.ClientId == clientId)
           .ExecuteDeleteAsync();

        _logger.LogInformation($"Orders cleared successfully for client with Id {clientId}");
    }

    public async Task RemoveAsync(long orderId)
    {
        var clientId = _clientContextReader.GetClientContextId();
        await _bookShopDbContext.Orders
            .Where(c => c.ClientId == clientId && c.Id == orderId)
            .ExecuteDeleteAsync();

        _logger.LogInformation($"Order with Id{orderId} remove for client with Id {clientId}");
    }
}
