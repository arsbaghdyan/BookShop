using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
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

        var order = await _bookShopDbContext.Orders.FirstOrDefaultAsync(o => o.ClientId == clientId && o.ProductId == orderAddModel.ProductId);

        var product = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == orderAddModel.ProductId);

        using (var transaction = _bookShopDbContext.Database.BeginTransaction())
        {
            try
            {
                var orderModel = new OrderModel();
                if (order != null)
                {
                    order.Count += orderAddModel.Count;
                    order.Amount = product.Price * order.Count;

                    await _bookShopDbContext.SaveChangesAsync();

                    _logger.LogInformation($"Order with Id{order.Id} added successefully for client with id {clientId}");

                    orderModel = _mapper.Map<OrderModel>(order);
                }

                var orderToAdd = _mapper.Map<OrderEntity>(orderAddModel);

                orderToAdd.Amount = product.Price * orderToAdd.Count;
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

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var orders = await _bookShopDbContext.Orders.Where(o => o.ClientId == clientId).ToListAsync();

        _bookShopDbContext.Orders.RemoveRange(orders);
        await _bookShopDbContext.SaveChangesAsync();

        _logger.LogInformation($"Orders cleared successfully for client with Id {clientId}");
    }

    public async Task RemoveAsync(long orderId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var order = await _bookShopDbContext.Orders.FirstOrDefaultAsync(o => o.ClientId == clientId && o.Id == orderId);

        _bookShopDbContext.Orders.Remove(order);
        await _bookShopDbContext.SaveChangesAsync();

        _logger.LogInformation($"Order with Id{order.Id} remove for client with Id {clientId}");
    }
}
