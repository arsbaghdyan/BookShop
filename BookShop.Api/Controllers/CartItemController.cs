using AutoMapper;
using BookShop.Data.Entities;
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
    private readonly IMapper _mapper;

    public CartItemController(ICartItemService cartItemService, IMapper mapper)
    {
        _cartItemService = cartItemService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<CartItemEntity>> AddItem(CartItemAddModel cartItemAddModel)
    {
        var cartItem = await _cartItemService.AddAsync(cartItemAddModel);

        return Ok(cartItem);
    }

    [HttpDelete]
    public async Task<ActionResult<CartItemEntity>> RemoveItem(long cartItemId)
    {
        await _cartItemService.RemoveAsync(cartItemId);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<CartItemEntity>> UpdateItem(CartItemUpdateModel cartItemUpdateModel)
    {
        var cartItem = await _cartItemService.UpdateAsync(cartItemUpdateModel);

        return Ok(cartItem);
    }
}
