using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CartItemModel>>> GetCartItems()
    {
        var cartItems = await _cartService.GetAllCartItemsAsync();

        return Ok(cartItems);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearAllItems()
    {
        await _cartService.ClearAsync();

        return Ok();
    }
}
