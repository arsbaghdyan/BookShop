﻿using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Data.Enums;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.BillingModels;
using BookShop.Services.Models.OrderModels;
using BookShop.Services.Models.PaymentModels;
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

    public record OrderInfo(long ProductId, int Count);

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
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Order with {orderToAdd.Id} Id is placed succesfully for '{clientId}' client.");

        var invoice = await _invoiceService.CreateInvoiceAsync(orderToAdd);

        var paymentRequest = new PaymentRequest<BankCardInfo>();

        var paymentResponse = new PaymentResponse();

        if (paymentRequest.Amount==invoice.TotalAmount)
        {
            paymentResponse.Result = PaymentResult.Success;
        }
        else
        {
            paymentResponse.Result = PaymentResult.Success;
        }

        var payment = await _paymentService.PayAsync(invoice.Id);

        var orderResult = new OrderModelWithPaymentResult
        {
            Order = _mapper.Map<OrderModel>(orderToAdd),
            PaymentMethodId = payment.PaymentMethodId,
            PaymentResult = paymentResponse.Result
        };

        return orderResult;
    }
}
