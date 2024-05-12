using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductAddModel>> AddProduct(ProductAddModel productAddModel)
    {
        await _productService.AddAsync(productAddModel);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<ProductUpdateModel>> UpdateProduct(ProductUpdateModel productUpdateModel)
    {
        await _productService.UpdateAsync(productUpdateModel);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> ClearProducts()
    {
        await _productService.ClearAsync();

        return Ok();
    }

    [HttpDelete("{productId}")]
    public async Task<ActionResult> RemoveProduct(long productId)
    {
        await _productService.RemoveAsync(productId);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ProductModel>>> GetAllProducts(long productId)
    {
        var products = await _productService.GetAllAsync(productId);

        return Ok(products);
    }

    [AllowAnonymous]
    [HttpGet("{productId}")]
    public async Task<ActionResult<ProductModel>> GetProduct(long productId)
    {
        var product = await _productService.GetByIdAsync(productId);

        return Ok(product);
    }
}
