using AutoMapper;
using BookShop.Api.Models.ProductModels;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
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
    public async Task<ActionResult<ProductEntity>> AddProduct(ProductAddModel productAddModel)
    {
        var productEntity = _mapper.Map<ProductEntity>(productAddModel);
        await _productService.AddAsync(productEntity);

        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ProductGetModel>> UpdateProduct(ProductUpdateModel productUpdateModel)
    {
        var productEntity = _mapper.Map<ProductEntity>(productUpdateModel);
        var updatedProduct = await _productService.UpdateAsync(productEntity);
        var productOutput = _mapper.Map<ProductGetModel>(updatedProduct);

        return Ok(productOutput);
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
    public async Task<ActionResult<List<ProductGetModel>>> GetAllProducts()
    {
        var products = await _productService.GetAllAsync();
        var productList = new List<ProductGetModel>();
        foreach (var product in productList)
        {
            var productOutput = _mapper.Map<ProductGetModel>(productList);
            productList.Add(productOutput);
        }

        return Ok(productList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductGetModel>> GetProduct(long id)
    {
        var product = await _productService.GetByIdAsync(id);
        var productOutput = _mapper.Map<ProductGetModel>(product);

        return Ok(productOutput);
    }
}
