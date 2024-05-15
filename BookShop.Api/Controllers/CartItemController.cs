using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpPost]
    public async Task<ActionResult<CartItemModel>> AddItem(CartItemAddModel cartItemAddModel)
    {
        var cart = await _cartItemService.AddAsync(cartItemAddModel);

        return Ok(cart);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveItem(long cartItemId)
    {
        await _cartItemService.RemoveAsync(cartItemId);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<CartItemModel>> UpdateItem(CartItemUpdateModel cartItemUpdateModel)
    {
        var cart = await _cartItemService.UpdateAsync(cartItemUpdateModel);

        return Ok(cart);
    }
}
