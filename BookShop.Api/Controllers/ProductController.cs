using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class ProductController : BaseClientAuthorizedController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ProductModel>>> GetAllProducts([FromQuery] ProductPageModel productPageModel)
    {
        var products = await _productService.GetAllAsync(productPageModel);

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
