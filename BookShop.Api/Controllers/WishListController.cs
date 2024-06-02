using BookShop.Api.Controllers.Base;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class WishListController : BaseClientAuthorizedController
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

    [HttpPost]
    public async Task<ActionResult<WishListItemEntity>> AddItem(WishListItemAddModel wishListItemAddModel)
    {
        var wishListItemToAdd = await _wishListService.AddAsync(wishListItemAddModel);

        return Ok(wishListItemToAdd);
    }

    [HttpDelete]
    public async Task<ActionResult<WishListItemEntity>> RemoveItem(long productId)
    {
        await _wishListService.RemoveAsync(productId);

        return Ok();
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAllItems()
    {
        await _wishListService.ClearAsync();

        return Ok();
    }
}
