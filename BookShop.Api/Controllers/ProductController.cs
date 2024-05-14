using BookShop.Api.Attributes;
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
    public async Task<ActionResult<ProductModel>> AddProduct(ProductAddModel productAddModel)
    {
        var product = await _productService.AddAsync(productAddModel);

        return Ok(product);
    }

    [HttpPut]
    public async Task<ActionResult<ProductModel>> UpdateProduct(ProductUpdateModel productUpdateModel)
    {
        var product = await _productService.UpdateAsync(productUpdateModel);

        return Ok(product);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearProducts()
    {
        await _productService.ClearAsync();

        return Ok();
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveProduct(long productId)
    {
        await _productService.RemoveAsync(productId);

        return Ok();
    }

    [ExcludeFromClientContextMiddleware]
    [HttpGet]
    public async Task<ActionResult<List<ProductModel>>> GetAllProducts()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    [ExcludeFromClientContextMiddleware]
    [HttpGet("{productId}")]
    public async Task<ActionResult<ProductModel>> GetProduct(long productId)
    {
        var product = await _productService.GetByIdAsync(productId);

        return Ok(product);
    }
}
