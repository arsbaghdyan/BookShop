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
    public async Task<ActionResult<CartItemAddVm>> AddItem(CartItemAddVm cartItemAddModel)
    {
        await _cartItemService.AddAsync(cartItemAddModel);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveItem(long cartId)
    {
        await _cartItemService.RemoveAsync(cartId);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<CartItemUpdateVm>> UpdateItem(CartItemUpdateVm cartItemUpdateModel)
    {
        await _cartItemService.UpdateAsync(cartItemUpdateModel);

        return Ok();
    }
}
