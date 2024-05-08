using AutoMapper;
using BookShop.Api.Models.ProductModels;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers;

[Authorize]
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

    [HttpPost]
    public async Task<ActionResult<ProductEntity>> AddProduct(ProductAddModel productAddModel)
    {
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult<ProductEntity>> UpdateProduct(ProductUpdateModel productUpdateModel)
    {
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult<ProductEntity>> ClearProducts()
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductEntity>> RemoveProduct(long productId)
    {
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductEntity>>> GetAllProducts()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductEntity>> GetProduct(long productId)
    {
        return Ok();
    }
}
