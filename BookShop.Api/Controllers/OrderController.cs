﻿using BookShop.Api.Controllers.Base;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.InvoiceModels;
using BookShop.Services.Models.OrderModel;
using BookShop.Services.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class OrderController : BaseAuthorizedController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<List<InvoiceModel>>> GetInvoice()
    {
        var orders = await _orderService.GetAllAsync();

        return Ok(orders);
    }

    [HttpGet("{paymentId}")]
    public async Task<ActionResult<InvoiceModel>> GetInvoiceById(long paymentId)
    {
        var order = await _orderService.GetByIdAsync(paymentId);

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderModel>> AddOrder(OrderAddModel orderAddModel)
    {
        var order = await _orderService.AddOrderAsync(orderAddModel);

        return Ok(order);
    }

    [HttpPost("From_Cart")]
    public async Task<ActionResult<OrderModel>> AddOrderFromCard(OrderAddFromCardModel orderAddFromCardModel)
    {
        var order = await _orderService.AddOrderFromCartAsync(orderAddFromCardModel);

        return Ok(order);
    }
}
