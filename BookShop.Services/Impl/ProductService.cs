﻿using AutoMapper;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

    public async Task AddAsync(ProductAddModel product)
    {
        if (product == null)
        {
            throw new Exception("There is nothing to add");
        }

        product.Details = SerializeDetails(product.Details);

        var productToAdd = _mapper.Map<ProductEntity>(product);

        _bookShopDbContext.Products.Add(productToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with Id {productToAdd.Id} added successfully.");
    }

    public async Task ClearAsync()
    {
        var products = await _bookShopDbContext.Products.ToListAsync();
        _bookShopDbContext.Products.RemoveRange(products);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation("All products cleared successfully.");
    }

    public async Task<List<ProductModel>> GetAllAsync(long productId)
    {
        var products = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        var productsToGet = _mapper.Map<List<ProductModel>>(products);

        return productsToGet;
    }

    public async Task<ProductModel> GetByIdAsync(long productId)
    {
        var product = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            throw new Exception($"Product not found");
        }

        var productsToGet = _mapper.Map<ProductModel>(product);

        return productsToGet;
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

    public async Task UpdateAsync(ProductUpdateModel product)
    {
        var productToUpdate = await GetByIdAsync(product.Id);

        product.Details = SerializeDetails(product.Details);

        productToUpdate.Name = product.Name;
        productToUpdate.Price = product.Price;
        productToUpdate.Manufacturer = product.Manufacturer;
        productToUpdate.Details = product.Details;
        productToUpdate.Count = product.Count;

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with Id {product.Id} updated successfully.");
    }

    private string SerializeDetails(string details)
    {
        return JsonConvert.SerializeObject(details);
    }
}
