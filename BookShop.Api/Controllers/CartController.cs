using BookShop.Api.Controllers.Base;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class CartController : ShopBaseController
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

    [HttpPost]
    public async Task<ActionResult<CartItemEntity>> AddItem(CartItemAddModel cartItemAddModel)
    {
        var cartItem = await _cartService.AddAsync(cartItemAddModel);

        return Ok(cartItem);
    }
    
    [HttpPut]
    public async Task<ActionResult<CartItemEntity>> UpdateItem(CartItemUpdateModel cartItemUpdateModel)
    {
        var cartItem = await _cartService.UpdateAsync(cartItemUpdateModel);

        return Ok(cartItem);
    }

    [HttpDelete]
    public async Task<ActionResult<CartItemEntity>> RemoveItem(long productId)
    {
        await _cartService.RemoveAsync(productId);

        return Ok();
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAllItems()
    {
        await _cartService.ClearAsync();

        return Ok();
    }
}
