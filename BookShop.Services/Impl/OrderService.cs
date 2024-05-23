using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.BillingModels;
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
    private readonly IBillingService _billingService;

    public record OrderInfo(long ProductId, int Count);

    public OrderService(IClientContextReader clientContextReader,
                        IMapper mapper,
                        ILogger<OrderService> logger,
                        BookShopDbContext bookShopDbContext,
                        IBillingService billingService)
    {
        _clientContextReader = clientContextReader;
        _mapper = mapper;
        _logger = logger;
        _bookShopDbContext = bookShopDbContext;
        _billingService = billingService;
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

    public async Task<OrderModelWithPaymentResult?> PlaceOrderAsync(OrderAddModel orderAddModel, long paymentMethodId)
    {
        var orderInfo = new OrderInfo(orderAddModel.ProductId, orderAddModel.Count);
        return await PlaceOrderInternalAsync(orderInfo, paymentMethodId);
    }

    public async Task<OrderModelWithPaymentResult?> PlaceOrderFromCartAsync(OrderAddFromCardModel orderAddFromCardModel, long paymentMethodId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cartItemEntity = await _bookShopDbContext.CartItems
            .Include(c => c.ProductEntity)
            .Where(c => c.CartEntity.ClientId == clientId)
            .FirstOrDefaultAsync(c => c.Id == orderAddFromCardModel.CartItemId);

        if (cartItemEntity == null)
        {
            throw new Exception($"Product with {cartItemEntity.ProductId} Id not found in cart for '{clientId}' client.");
        }

        var orderInfo = new OrderInfo(cartItemEntity.ProductId, cartItemEntity.Count);
        var order = await PlaceOrderInternalAsync(orderInfo, paymentMethodId);

        _bookShopDbContext.CartItems.Remove(cartItemEntity);
        await _bookShopDbContext.SaveChangesAsync();

        return order;
    }

    private async Task<OrderModelWithPaymentResult?> PlaceOrderInternalAsync(OrderInfo productInfo, long paymentMethodId)
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
                                    .FirstOrDefaultAsync(p => p.ClientId == clientId && p.Id == paymentMethodId);

        if (paymentMethod == null)
        {
            throw new Exception($"Payment method not found for '{clientId}' client.");
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
        _logger.LogInformation($"Order with {orderToAdd.Id} Id is placed succesfully for '{clientId}' client.");

        var paymentRequest = new PaymentRequest<BankCardInfo>();

        paymentRequest.Amount = invoice.TotalAmount;

        var paymentResponse = await _billingService.PayViaCardAsync(paymentRequest);

        var orderResult = new OrderModelWithPaymentResult
        {
            Order = _mapper.Map<OrderModel>(orderToAdd),
            PaymentResult = paymentResponse.Result,
            PaymentMethodId = paymentMethod.Id
        };

        return orderResult;
    }
}
