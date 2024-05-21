using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
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
    public async Task<ActionResult<List<OrderModel>>> GetOrders()
    {
        var orders = await _orderService.GetAllAsync();

        return Ok(orders);
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderModel>> GetOrderById(long orderId)
    {
        var order = await _orderService.GetByIdAsync(orderId);

        return Ok(order);
    }

    [HttpPost("placeOrder")]
    public async Task<ActionResult<OrderModel>> AddOrder(OrderAddModel orderAddModel)
    {
        var order = await _orderService.PlaceOrderAsync(orderAddModel);

        return Ok(order);
    }

    [HttpPost("placeOrderFromCart")]
    public async Task<ActionResult<OrderModel>> AddOrderFromCard(OrderAddFromCardModel orderAddFromCardModel)
    {
        var order = await _orderService.PlaceOrderFromCartAsync(orderAddFromCardModel);

        return Ok(order);
    }
}
