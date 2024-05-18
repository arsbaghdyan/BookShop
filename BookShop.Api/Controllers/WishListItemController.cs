using BookShop.Api.Controllers.Base;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class WishListItemController : BaseAuthorizedController
{
    private readonly IWishListItemService _wishListItemService;

    public WishListItemController(IWishListItemService wishListItemService)
    {
        _wishListItemService = wishListItemService;
    }

    [HttpPost]
    public async Task<ActionResult<WishListItemEntity>> AddItem(WishListItemAddModel wishListItemAddModel)
    {
        var wishListItemToAdd = await _wishListItemService.AddAsync(wishListItemAddModel);

        return Ok(wishListItemToAdd);
    }

    [HttpDelete]
    public async Task<ActionResult<WishListItemEntity>> RemoveItem(long wishListId)
    {
        await _wishListItemService.RemoveAsync(wishListId);

        return Ok();
    }
}
