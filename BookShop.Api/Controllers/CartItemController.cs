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
        var cartItemToAdd = _mapper.Map<CartItemEntity>(cartItemAddModel);
        await _cartItemService.AddAsync(cartItemToAdd);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<CartItemEntity>> RemoveItem(CartItemDeleteModel cartItemDeleteModel)
    {
        var cartItemToRemove = _mapper.Map<CartItemEntity>(cartItemDeleteModel);
        await _cartItemService.RemoveAsync(cartItemToRemove);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<CartItemEntity>> UpdateItem(CartItemUpdateModel cartItemUpdateModel)
    {
        var cartItemToUpdate = _mapper.Map<CartItemEntity>(cartItemUpdateModel);
        await _cartItemService.UpdateAsync(cartItemToUpdate);

        return Ok();
    }
}
