using AutoMapper;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class ProductService : IProductService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<ProductService> _logger;
    private readonly IMapper _mapper;

    public ProductService(BookShopDbContext bookShopDbContext, ILogger<ProductService> logger, IMapper mapper)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ProductModel> AddAsync(ProductAddModel product)
    {
        var productToAdd = _mapper.Map<ProductEntity>(product);

        _bookShopDbContext.Products.Add(productToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with Id {productToAdd.Id} added successfully.");

        var productModel = _mapper.Map<ProductModel>(productToAdd);

        return productModel;
    }

    public async Task ClearAsync()
    {
        var products = await _bookShopDbContext.Products.ToListAsync();
        _bookShopDbContext.Products.RemoveRange(products);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation("All products cleared successfully.");
    }

    public async Task<List<ProductModel>> GetAllAsync()
    {
        var products = await _bookShopDbContext.Products.ToListAsync();

        var productsToGet = _mapper.Map<List<ProductModel>>(products);

        return productsToGet;
    }

    public async Task<ProductModel> GetByIdAsync(long productId)
    {
        var product = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        var productsToGet = _mapper.Map<ProductModel>(product);

        return productsToGet;
    }

    public async Task RemoveAsync(long productId)
    {
        var product = _bookShopDbContext.Products.FirstOrDefault(s => s.Id == productId);

        _bookShopDbContext.Products.Remove(product);
        await _bookShopDbContext.SaveChangesAsync();
    }

    public async Task<ProductModel> UpdateAsync(ProductUpdateModel product)
    {
        var productToUpdate = await GetByIdAsync(product.Id);

        productToUpdate.Name = product.Name;
        productToUpdate.Price = product.Price;
        productToUpdate.Manufacturer = product.Manufacturer;
        productToUpdate.Details = product.Details;
        productToUpdate.Count = product.Count;

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with Id {product.Id} updated successfully.");

        var productModel = _mapper.Map<ProductModel>(productToUpdate);

        return productModel;
    }
}
