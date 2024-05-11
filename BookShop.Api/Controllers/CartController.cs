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
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public CartController(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<CartEntity>> Create(CartCreateModel cartCreateModel)
    {
        var cart = _mapper.Map<CartEntity>(cartCreateModel);
        await _cartService.CreateAsync(cart.ClientId);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<CartItemGetVm>>> GetCartItems(long id)
    {
        var cartItems = await _cartService.GetAllCartItemsAsync(id);
        var cartItemsOutput = new List<CartItemGetVm>();
        foreach (var cartItem in cartItems)
        {
            var cartItemOutput = _mapper.Map<CartItemGetVm>(cartItem);
            cartItemsOutput.Add(cartItemOutput);
        }

        return Ok(cartItemsOutput);
    }
}
