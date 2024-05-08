using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        try
        {
            _bookShopDbContext.Products.Add(productEntity);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Product with Id {productEntity.Id} added successfully.");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while adding product.");
        }
    }

    public async Task ClearAsync()
    {
        try
        {
            var products = await _bookShopDbContext.Products.ToListAsync();
            _bookShopDbContext.Products.RemoveRange(products);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation("All products cleared successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while clearing products.");
            throw;
        }
    }

    public Task<List<ProductEntity>> GetAllAsync()
    {
        try
        {
            return _bookShopDbContext.Products.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all products.");
            throw;
        }
    }

    public async Task<ProductEntity> GetByIdAsync(long productId)
    {
        try
        {
            var product = await _bookShopDbContext.Products
                                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new Exception($"Product not found");
            }

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving product with Id {productId}.");
            throw;
        }
    }

    public async Task RemoveAsync(long productId)
    {
        try
        {
            var product = _bookShopDbContext.Products.FirstOrDefault(s => s.Id == productId);
            if (product == null)
            {
                throw new Exception($"Product not found");
            }
            _bookShopDbContext.Products.Remove(product);
            await _bookShopDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving product with Id {productId}.");
            throw;
        }
    }

    public async Task<ProductEntity> UpdateAsync(ProductEntity productEntity)
    {
        try
        {
            var productToUpdate = await GetByIdAsync(productEntity.Id);

            productToUpdate.Name = productEntity.Name;
            productToUpdate.Price = productEntity.Price;
            productToUpdate.Manufacturer = productEntity.Manufacturer;
            productToUpdate.Details = productEntity.Details;
            productToUpdate.Count = productEntity.Count;

            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Product with Id {productEntity.Id} updated successfully.");
            return productToUpdate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating product with Id {productEntity.Id}.");
            throw;
        }
    }
}
