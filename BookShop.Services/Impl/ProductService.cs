﻿using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
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

    public async Task<List<ProductModel>> GetAllAsync()
    {
        var productEntities = await _bookShopDbContext.Products.ToListAsync();

        var productsModels = _mapper.Map<List<ProductModel>>(productEntities);

        return productsModels;
    }

    public async Task<ProductModel> GetByIdAsync(long productId)
    {
        var productEntity = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        var productsModel = _mapper.Map<ProductModel>(productEntity);

        return productsModel;
    }

    public async Task<ProductModel> AddAsync(ProductAddModel productAddModel)
    {
        if (productAddModel.Count <= 0)
        {
            throw new Exception("Product count can't be less than 0");
        }

        var productCheck = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Manufacturer == productAddModel.Manufacturer
        && p.Details == p.Details && p.Name == productAddModel.Name && p.Price == productAddModel.Price);

        var productEntity = new ProductEntity();

        var productModel = new ProductModel();
        if (productCheck != null)
        {
            productCheck.Count += productAddModel.Count;
            await _bookShopDbContext.SaveChangesAsync();

            _logger.LogInformation($"Product with Id {productCheck.Id} added successfully");

            productModel = _mapper.Map<ProductModel>(productCheck);

            return productModel;
        }
        productEntity = _mapper.Map<ProductEntity>(productAddModel);
        _bookShopDbContext.Products.Add(productEntity);

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with Id {productEntity.Id} added successfully");

        productModel = _mapper.Map<ProductModel>(productEntity);

        return productModel;
    }

    public async Task<ProductModel> UpdateAsync(ProductUpdateModel product)
    {
        if (product.Count <= 0)
        {
            throw new Exception("Product count can't be less than 0");
        }

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

    public async Task ClearAsync()
    {
        await _bookShopDbContext.Products.ExecuteDeleteAsync();

        _logger.LogInformation("All products cleared successfully.");
    }

    public async Task RemoveAsync(long productId)
    {
        await _bookShopDbContext.Products.Where(p=>p.Id== productId).ExecuteDeleteAsync();

        _logger.LogInformation($"Product with Id {productId} removed successfully.");
    }
}
