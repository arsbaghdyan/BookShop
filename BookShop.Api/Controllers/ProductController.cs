﻿using AutoMapper;
using BookShop.Data.Entities;
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
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProductEntity>> AddProduct(ProductAddVm productAddModel)
    {
        var productEntity = _mapper.Map<ProductEntity>(productAddModel);
        await _productService.AddAsync(productEntity);

        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ProductEntity>> UpdateProduct(ProductUpdateVm productUpdateModel)
    {
        var productEntity = _mapper.Map<ProductEntity>(productUpdateModel);
        await _productService.UpdateAsync(productEntity);

        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<ProductEntity>> ClearProducts()
    {
        await _productService.ClearAsync();

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductEntity>> RemoveProduct(long id)
    {
        await _productService.RemoveAsync(id);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductGetVm>>> GetAllProducts()
    {
        var products = await _productService.GetAllAsync();
        var productList = new List<ProductGetVm>();
        foreach (var product in productList)
        {
            var productOutput = _mapper.Map<ProductGetVm>(productList);
            productList.Add(productOutput);
        }

        return Ok(productList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductGetVm>> GetProduct(long id)
    {
        var product = await _productService.GetByIdAsync(id);
        var productOutput = _mapper.Map<ProductGetVm>(product);

        return Ok(productOutput);
    }
}
