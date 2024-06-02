using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class OrderController : ShopBaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderModelWithPaymentResult>>> GetOrders()
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
    public async Task<ActionResult<OrderModelWithPaymentResult>> AddOrder(List<OrderAddModel> orderAddModels)
    {
        var order = await _orderService.PlaceOrderAsync(orderAddModels);

        return Ok(order);
    }

    [HttpPost("placeOrderFromCart")]
    public async Task<ActionResult<OrderModelWithPaymentResult>> AddOrderFromCard(List<OrderAddFromCartModel> orderAddFromCardModels)
    {
        var order = await _orderService.PlaceOrderFromCartAsync(orderAddFromCardModels);

        return Ok(order);
    }
}
