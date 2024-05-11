using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProductAddVm>> AddProduct(ProductAddVm productAddModel)
    {
        await _productService.AddAsync(productAddModel);

        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ProductUpdateVm>> UpdateProduct(ProductUpdateVm productUpdateModel)
    {
        await _productService.UpdateAsync(productUpdateModel);

        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> ClearProducts()
    {
        await _productService.ClearAsync();

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveProduct(long productId)
    {
        await _productService.RemoveAsync(productId);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductGetVm>>> GetAllProducts(long productId)
    {
        var products = await _productService.GetAllAsync(productId);

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductGetVm>> GetProduct(long id)
    {
        var product = await _productService.GetByIdAsync(id);

        return Ok(product);
    }
}
