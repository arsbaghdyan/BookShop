using BookShop.Services.Abstractions;
using BookShop.Services.Models.OrderModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderModel>> AddOrder(OrderAddModel orderAddModel)
    {
        var order = await _orderService.AddOrderAsync(orderAddModel);

        return Ok(order);
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Remove(long orderId)
    {
        await _orderService.RemoveAsync(orderId);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Clear()
    {
        await _orderService.ClearAsync();

        return Ok();
    }
}
