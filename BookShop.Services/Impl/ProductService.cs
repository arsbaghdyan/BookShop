using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookShop.Services.Impl;

internal class ProductService : IProductService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<ProductService> _logger;

    public ProductService(BookShopDbContext bookShopDbContext, ILogger<ProductService> logger)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
    }

    public async Task AddAsync(ProductEntity productEntity)
    {
        if (productEntity == null)
        {
            throw new Exception("There is nothing to add");
        }

        productEntity.Details = SerializeDetails(productEntity.Details);

        _bookShopDbContext.Products.Add(productEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with Id {productEntity.Id} added successfully.");
    }

    public async Task ClearAsync()
    {
        var products = await _bookShopDbContext.Products.ToListAsync();
        _bookShopDbContext.Products.RemoveRange(products);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation("All products cleared successfully.");
    }

    public async Task<List<ProductEntity>> GetAllAsync()
    {
        return await _bookShopDbContext.Products.ToListAsync();
    }

    public async Task<ProductEntity> GetByIdAsync(long productId)
    {
        var product = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            throw new Exception($"Product not found");
        }

        return product;
    }

    public async Task RemoveAsync(long productId)
    {
        var product = _bookShopDbContext.Products.FirstOrDefault(s => s.Id == productId);

        if (product == null)
        {
            throw new Exception($"Product not found");
        }

        _bookShopDbContext.Products.Remove(product);
        await _bookShopDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductEntity productEntity)
    {
        var productToUpdate = await GetByIdAsync(productEntity.Id);

        productEntity.Details = SerializeDetails(productEntity.Details);

        productToUpdate.Name = productEntity.Name;
        productToUpdate.Price = productEntity.Price;
        productToUpdate.Manufacturer = productEntity.Manufacturer;
        productToUpdate.Details = productEntity.Details;
        productToUpdate.Count = productEntity.Count;

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with Id {productEntity.Id} updated successfully.");
    }

    private string SerializeDetails(string details)
    {
        return JsonConvert.SerializeObject(details);
    }
}
