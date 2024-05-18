using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class WishListController : BaseAuthorizedController
{
    private readonly IWishListService _wishListService;

    public WishListController(IWishListService wishListService)
    {
        _wishListService = wishListService;
    }

    [HttpGet]
    public async Task<ActionResult<List<WishListItemModel>>> GetWishlistItems()
    {
        var wishListItems = await _wishListService.GetAllWishListItemsAsync();

        return Ok(wishListItems);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearAllItems()
    {
        await _wishListService.ClearAsync();

        return Ok();
    }
}
