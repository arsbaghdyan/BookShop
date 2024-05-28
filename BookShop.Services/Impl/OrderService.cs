using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Data.Enums;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.InvoiceModels;
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
    private readonly IInvoiceService _invoiceService;
    private readonly IPaymentService _paymentService;

    public record OrderInfo(long ProductId, int Count, long PaymentMethodId);

    public OrderService(IClientContextReader clientContextReader,
                        IMapper mapper,
                        ILogger<OrderService> logger,
                        BookShopDbContext bookShopDbContext,
                        IInvoiceService invoiceService,
                        IPaymentService paymentService)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _logger = logger;
        _bookShopDbContext = bookShopDbContext;
        _invoiceService = invoiceService;
        _paymentService = paymentService;
    }

    public async Task<List<OrderModel?>> GetAllAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderEntities = await _bookShopDbContext.Orders
            .Where(i => i.ClientId == clientId)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ToListAsync();

        return _mapper.Map<List<OrderModel?>>(orderEntities);
    }

    public async Task<OrderModel?> GetByIdAsync(long orderId)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderEntity = await _bookShopDbContext.Orders
            .Where(i => i.ClientId == clientId)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(i => i.Id == orderId);

        return _mapper.Map<OrderModel?>(orderEntity);
    }

    public async Task<List<OrderModelWithPaymentResult>> PlaceOrderAsync(List<OrderAddModel> orderAddModels)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderModels = new List<OrderModelWithPaymentResult>();

        foreach (var orderAddModel in orderAddModels)
        {
            var orderInfos = orderAddModel.OrderItems
                .Select(orderItemModel => new OrderInfo(orderItemModel.ProductId, orderItemModel.Count, orderAddModel.PaymentMethodId))
                .ToList();

            var order = await PlaceOrderInternalAsync(orderInfos);
            orderModels.Add(order);
        }

        return orderModels;
    }

    public async Task<List<OrderModelWithPaymentResult>> PlaceOrderFromCartAsync(List<OrderAddFromCartModel> orderAddFromCardModels)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderModels = new List<OrderModelWithPaymentResult>();

        foreach (var orderAddFromCardModel in orderAddFromCardModels)
        {
            var orderInfos = new List<OrderInfo>();

            foreach (var cartItemId in orderAddFromCardModel.CartItemIds)
            {
                var cartItemEntity = await _bookShopDbContext.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.Cart.ClientId == clientId)
                    .FirstOrDefaultAsync(c => c.Id == cartItemId);

                if (cartItemEntity == null)
                {
                    throw new Exception($"Product with {cartItemId} Id not found in cart for '{clientId}' client.");
                }

                var orderInfo = new OrderInfo(cartItemEntity.ProductId, cartItemEntity.Count, orderAddFromCardModel.PaymentMethodId);
                orderInfos.Add(orderInfo);

                _bookShopDbContext.CartItems.Remove(cartItemEntity);
            }

            await _bookShopDbContext.SaveChangesAsync();

            var order = await PlaceOrderInternalAsync(orderInfos);
            orderModels.Add(order);
        }

        return orderModels;
    }

    private async Task<OrderModelWithPaymentResult?> PlaceOrderInternalAsync(List<OrderInfo> orderInfoList)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderProducts = new List<OrderProduct>();

        decimal totalAmount = 0;

        foreach (var orderInfo in orderInfoList)
        {
            var productEntity = await _bookShopDbContext.Products
                .FirstOrDefaultAsync(p => p.Id == orderInfo.ProductId);

            if (productEntity == null)
            {
                throw new Exception($"Product with Id {orderInfo.ProductId} not found.");
            }

            if (productEntity.Count < orderInfo.Count)
            {
                throw new Exception("Not enough product");
            }

            totalAmount += productEntity.Price * orderInfo.Count;

            orderProducts.Add(new OrderProduct
            {
                ProductId = orderInfo.ProductId,
                Product = productEntity
            });
        }

        var paymentMethod = await _bookShopDbContext.PaymentMethods
            .FirstOrDefaultAsync(p => p.ClientId == clientId);

        if (paymentMethod == null)
        {
            throw new Exception($"Payment method not found for '{clientId}' client.");
        }

        InvoiceModel invoice;
        OrderEntity order;

        using (var transaction = await _bookShopDbContext.Database.BeginTransactionAsync())
        {
            try
            {
                order = new OrderEntity
                {
                    ClientId = clientId,
                    PaymentMethodId = paymentMethod.Id,
                    Amount = totalAmount,
                    Count = orderInfoList.Sum(orderInfo => orderInfo.Count),
                    OrderProducts = orderProducts
                };

                _bookShopDbContext.Orders.Add(order);
                await _bookShopDbContext.SaveChangesAsync();

                foreach (var orderInfo in orderInfoList)
                {
                    var productEntity = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == orderInfo.ProductId);
                    productEntity.Count -= orderInfo.Count;
                }

                await _bookShopDbContext.SaveChangesAsync();
                _logger.LogInformation($"Order with Id {order.Id} placed successfully for client '{clientId}'.");

                invoice = await _invoiceService.CreateInvoiceAsync(order);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        var payment = await _paymentService.PayAsync(invoice.Id);

        return new OrderModelWithPaymentResult
        {
            Order = _mapper.Map<OrderModel>(order),
            PaymentMethodId = payment.PaymentMethodId,
            PaymentResult = payment.PaymentStatus
        };
    }
}
