using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
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
            .ToListAsync();

        return _mapper.Map<List<OrderModel?>>(orderEntities);
    }

    public async Task<OrderModel?> GetByIdAsync(long orderId)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderEntity = await _bookShopDbContext.Orders
            .Where(i => i.ClientId == clientId)
            .FirstOrDefaultAsync(i => i.Id == orderId);

        return _mapper.Map<OrderModel?>(orderEntity);
    }

    public async Task<OrderModelWithPaymentResult?> PlaceOrderAsync(OrderAddModel orderAddModel)
    {
        var orderInfo = new OrderInfo(orderAddModel.ProductId, orderAddModel.Count, orderAddModel.PaymentMethodId);
        return await PlaceOrderInternalAsync(orderInfo);
    }

    public async Task<OrderModelWithPaymentResult?> PlaceOrderFromCartAsync(OrderAddFromCartModel orderAddFromCardModel)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cartItemEntity = await _bookShopDbContext.CartItems
            .Include(c => c.Product)
            .Where(c => c.Cart.ClientId == clientId)
            .FirstOrDefaultAsync(c => c.Id == orderAddFromCardModel.CartItemId);

        if (cartItemEntity == null)
        {
            throw new Exception($"Product with {cartItemEntity.ProductId} Id not found in cart for '{clientId}' client.");
        }

        var orderInfo = new OrderInfo(cartItemEntity.ProductId, cartItemEntity.Count, orderAddFromCardModel.PaymentMethodId);
        var order = await PlaceOrderInternalAsync(orderInfo);

        _bookShopDbContext.CartItems.Remove(cartItemEntity);
        await _bookShopDbContext.SaveChangesAsync();

        return order;
    }

    private async Task<OrderModelWithPaymentResult?> PlaceOrderInternalAsync(OrderInfo productInfo)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var orderEntity = await _bookShopDbContext.Orders
            .Where(o => o.ClientId == clientId)
            .FirstOrDefaultAsync(o => o.ProductId == productInfo.ProductId);

        var productEntity = await _bookShopDbContext.Products
            .FirstOrDefaultAsync(p => p.Id == productInfo.ProductId);

        if (orderEntity == null || productEntity == null)
        {
            throw new Exception($"Input parametr for productId {productInfo.ProductId} is invalid");
        }

        var paymentMethod = await _bookShopDbContext.PaymentMethods
            .FirstOrDefaultAsync(p => p.ClientId == clientId && p.Id == productInfo.PaymentMethodId);

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
                order = _mapper.Map<OrderEntity>(productInfo);

                order.Amount = productEntity.Price * order.Count;
                order.ClientId = clientId;

                _bookShopDbContext.Orders.Add(order);
                await _bookShopDbContext.SaveChangesAsync();
                _logger.LogInformation($"Order with {order.Id} Id is placed succesfully for '{clientId}' client.");

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

        var orderResult = new OrderModelWithPaymentResult
        {
            Order = _mapper.Map<OrderModel>(order),
            PaymentMethodId = payment.PaymentMethodId,
            PaymentResult = payment.PaymentStatus
        };

        return orderResult;
    }
}
