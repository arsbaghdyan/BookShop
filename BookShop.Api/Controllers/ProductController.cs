﻿using BookShop.Api.Controllers.Base;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Route("[controller]")]
public class ProductController : BaseAuthorizedController
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

    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveProduct(long productId)
    {
        await _productService.RemoveAsync(productId);

        return Ok();
    }
}
