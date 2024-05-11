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
public class WishListController : ControllerBase
{
    private readonly IWishListService _wishlistService;
    private readonly IMapper _mapper;

    public WishListController(IWishListService wishlistService, IMapper mapper)
    {
        _wishlistService = wishlistService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<WishListEntity>> Create(CartCreateModel cartCreateModel)
    {
        var cart = _mapper.Map<CartEntity>(cartCreateModel);
        await _wishlistService.CreateAsync(cart.ClientId);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<WishListItemGetModel>>> GetWishlistItems(long id)
    {
        var wishlistItems = await _wishlistService.GetAllWishListItemsAsync(id);
        var wishlistItemsOutput = new List<WishListItemGetModel>();
        foreach (var wishlistItem in wishlistItems)
        {
            var wishlistItemOutput = _mapper.Map<WishListItemGetModel>(wishlistItem);
            wishlistItemsOutput.Add(wishlistItemOutput);
        }

        return Ok(wishlistItemsOutput);
    }
}
