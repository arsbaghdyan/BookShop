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

    public WishListItemController(IWishListItemService wishlistItemService)
    {
        _wishlistItemService = wishlistItemService;
    }

    [HttpPost]
    public async Task<ActionResult<WishListItemAddVm>> AddItem(WishListItemAddVm wishListItemAddModel)
    {
        await _wishlistItemService.AddAsync(wishListItemAddModel);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveItem(long wishlistItemId)
    {
        await _wishlistItemService.RemoveAsync(wishlistItemId);

        return Ok();
    }
}
