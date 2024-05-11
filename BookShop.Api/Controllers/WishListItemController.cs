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
public class WishListItemController : ControllerBase
{
    private readonly IWishListItemService _wishlistItemService;
    private readonly IMapper _mapper;

    public WishListItemController(IWishListItemService wishlistItemService, IMapper mapper)
    {
        _wishlistItemService = wishlistItemService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<WishListItemEntity>> AddItem(WishListItemAddModel wishListItemAddModel)
    {
        var wishlistItemToAdd = _mapper.Map<WishListItemEntity>(wishListItemAddModel);
        await _wishlistItemService.AddAsync(wishlistItemToAdd);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<WishListItemEntity>> RemoveItem(WishListItemDeleteModel wishListItemDeleteModel)
    {
        var wishlistItemToRemove = _mapper.Map<WishListItemEntity>(wishListItemDeleteModel);
        await _wishlistItemService.RemoveAsync(wishlistItemToRemove);

        return Ok();
    }
}
