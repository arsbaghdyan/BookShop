using AutoMapper;
using BookShop.Api.Models.ProductModels;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public async Task<ActionResult<ProductEntity>> AddProduct(ProductAddModel productAddModel)
    {
        var productEntity = _mapper.Map<ProductEntity>(productAddModel);
        await _productService.AddAsync(productEntity);

        return Ok(productEntity);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ProductEntity>> UpdateProduct(ProductUpdateModel productUpdateModel)
    {
        var productEntity = _mapper.Map<ProductEntity>(productUpdateModel);
        var updatedProduct = await _productService.UpdateAsync(productEntity);

        return Ok(updatedProduct);
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
    public async Task<ActionResult> RemoveProduct(long id)
    {
        await _productService.RemoveAsync(id);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductEntity>>> GetAllProducts()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductEntity>> GetProduct(long id)
    {
        var product = await _productService.GetByIdAsync(id);

        return Ok(product);
    }

    //private string DeserializeDetails(string detailsJson)
    //{
    //    return JsonConvert.DeserializeObject<string>(detailsJson);
    //}
}
