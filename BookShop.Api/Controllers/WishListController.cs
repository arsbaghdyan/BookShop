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

    public WishListController(IWishListService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpGet]
    public async Task<ActionResult<List<WishListItemModel>>> GetWishlistItems(long wishListItemId)
    {
        var wishlistItems = await _wishlistService.GetAllWishListItemsAsync(wishListItemId);

        return Ok(wishlistItems);
    }
}
