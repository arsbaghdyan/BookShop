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
        var clientId = _clientContextReader.GetClientContextId();
        var orderEntity = await _bookShopDbContext.Orders
            .FirstOrDefaultAsync(o => o.ClientId == clientId && o.ProductId == orderAddModel.ProductId);
        var productEntity = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == orderAddModel.ProductId);

        if (orderEntity == null || productEntity == null)
        {
            throw new Exception($"Input parametr for productId {orderAddModel.ProductId} is invalid");
        }

        using (var transaction = _bookShopDbContext.Database.BeginTransaction())
        {
            try
            {
                var orderModel = new OrderModel();
                if (orderEntity != null)
                {
                    orderEntity.Count += orderAddModel.Count;
                    orderEntity.Amount = productEntity.Price * orderEntity.Count;

                    await _bookShopDbContext.SaveChangesAsync();

                    _logger.LogInformation($"Order with Id{orderEntity.Id} added successefully for client with id {clientId}");

                    orderModel = _mapper.Map<OrderModel>(orderEntity);
                }

                var orderToAdd = _mapper.Map<OrderEntity>(orderAddModel);

                orderToAdd.Amount = productEntity.Price * orderToAdd.Count;
                orderToAdd.ClientId = clientId;

                _bookShopDbContext.Orders.Add(orderToAdd);
                await _bookShopDbContext.SaveChangesAsync();
                _logger.LogInformation($"Order with Id{orderToAdd.Id} added successefully for client with id {clientId}");

                var invoice = new InvoiceEntity
                {
                    ClientId = clientId,
                    CreatedAt = DateTime.UtcNow,
                    OrderId = orderToAdd.Id,
                    TotalAmount = orderToAdd.Amount,
                };

                _bookShopDbContext.Invoices.Add(invoice);
                await _bookShopDbContext.SaveChangesAsync();

                orderModel = _mapper.Map<OrderModel>(orderToAdd);

                transaction.Commit();

                return orderModel;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError($"Error {ex.Message}");

                throw new Exception($"Error {ex.Message}");
            }
        }
    }

    public async Task<OrderModel> AddOrderFromCartAsync(OrderAddFromCardModel orderAddFromCardModel)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var cartItemEntity = await _bookShopDbContext.CartItems.Include(c => c.ProductEntity)
            .FirstOrDefaultAsync(c => c.Id == orderAddFromCardModel.CartItemId && c.CartEntity.ClientId == clientId);

        if (cartItemEntity == null)
        {
            throw new Exception($"CartItem with Id {orderAddFromCardModel.CartItemId} not found for client with Id {clientId}");
        }

        using (var transaction = _bookShopDbContext.Database.BeginTransaction())
        {
            try
            {
                var orderToAdd = new OrderEntity
                {
                    ClientId = clientId,
                    ProductId = cartItemEntity.ProductId,
                    Count = cartItemEntity.Count,
                    Amount = cartItemEntity.Price
                };

                _bookShopDbContext.Orders.Add(orderToAdd);
                await _bookShopDbContext.SaveChangesAsync();
                _logger.LogInformation($"Order added successfully for client with Id {clientId}");

                var invoice = new InvoiceEntity
                {
                    ClientId = clientId,
                    CreatedAt = DateTime.UtcNow,
                    OrderId = orderToAdd.Id,
                    TotalAmount = orderToAdd.Amount,
                };

                _bookShopDbContext.Invoices.Add(invoice);
                await _bookShopDbContext.SaveChangesAsync();

                var orderModel = _mapper.Map<OrderModel>(orderToAdd);

                return orderModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");
                throw new Exception($"Error {ex.Message}");
            }
        }
    }

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderEntities = await _bookShopDbContext.Orders.Where(o => o.ClientId == clientId).ToListAsync();

        _bookShopDbContext.Orders.RemoveRange(orderEntities);
        await _bookShopDbContext.SaveChangesAsync();

        _logger.LogInformation($"Orders cleared successfully for client with Id {clientId}");
    }

    public async Task RemoveAsync(long orderId)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderEntity = await _bookShopDbContext.Orders.FirstOrDefaultAsync(o => o.ClientId == clientId && o.Id == orderId);

        _bookShopDbContext.Orders.Remove(orderEntity);
        await _bookShopDbContext.SaveChangesAsync();

        _logger.LogInformation($"Order with Id{orderEntity.Id} remove for client with Id {clientId}");
    }
}
