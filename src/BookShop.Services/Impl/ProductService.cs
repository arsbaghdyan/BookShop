using AutoMapper;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Repositories.Impl;
using BookShop.Repositories.Interfaces;
using BookShop.Services.Abstractions;
using BookShop.Services.Exceptions;
using BookShop.Services.Helper;
using BookShop.Services.Models.PageModels;
using BookShop.Services.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Linq.Expressions;

namespace BookShop.Services.Impl;

public class ProductService : IProductService
{
    private readonly IRepository<ProductEntity> _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly IConnectionMultiplexer? _connectionMultiplexer;

    public ProductService(
        IRepository<ProductEntity> productRepository,
        IMapper mapper,
        ILogger<ProductService> logger,
        IConnectionMultiplexer? connectionMultiplexer)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<PagedList<ProductModel?>> GetAllAsync(ProductPageModel productPageModel)
    {
        PagedList<ProductModel?> cachedProducts = null;
        if (_connectionMultiplexer != null && _connectionMultiplexer.IsConnected)
        {
            try
            {
                var db = _connectionMultiplexer.GetDatabase();
                var cachedData = await db.StringGetAsync("Products");
                if (cachedData.HasValue)
                {
                    cachedProducts = JsonConvert.DeserializeObject<PagedList<ProductModel?>>(cachedData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving data from Redis");
            }
        }

        if (cachedProducts != null)
        {
            return cachedProducts;
        }

        var productQuery = await _productRepository.GetAllAsync();

        Expression<Func<ProductEntity, object>> keySelector = productPageModel.OrderBy?.ToLower() switch
        {
            "id" => p => p.Id,
            "name" => p => p.Name,
            "price" => p => p.Price,
            "manufacturer" => p => p.Manufacturer,
            "count" => p => p.Count,
            _ => p => p.Name,
        };

        if (productPageModel.IsOrderAsc)
        {
            productQuery.OrderBy(keySelector);
        }
        else
        {
            productQuery.OrderByDescending(keySelector);
        }

        var productEntities = await PagedList<ProductEntity>
            .ToPagedListAsync(productQuery, productPageModel.PageNumber, productPageModel.PageSize);

        var productModels = _mapper.Map<List<ProductModel?>>(productEntities.Items);

        var paginatedProducts = new PagedList<ProductModel?>(productModels, productEntities.TotalCount, productEntities.CurrentPage, productEntities.PageSize);

        if (_connectionMultiplexer != null && _connectionMultiplexer.IsConnected)
        {
            try
            {
                var db = _connectionMultiplexer.GetDatabase();
                await db.StringSetAsync("Products", JsonConvert.SerializeObject(paginatedProducts), TimeSpan.FromMinutes(2));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting data to Redis");
            }
        }

        return paginatedProducts;
    }

    public async Task<ProductModel?> GetByIdAsync(long productId)
    {
        var productEntity = await _productRepository.GetByIdAsync(productId);

        return _mapper.Map<ProductModel?>(productEntity);
    }

    public async Task<ProductModel?> AddAsync(ProductAddModel productAddModel)
    {
        if (productAddModel.Count <= 0)
        {
            throw new InvalidProductCountException("Product count can't be less or equal 0");
        }

        var products = await _productRepository.GetAllAsync();

        var productCheck = await products
            .FirstOrDefaultAsync(p => p.Manufacturer == productAddModel.Manufacturer &&
            p.Name == productAddModel.Name && p.Price == productAddModel.Price);

        var productEntity = new ProductEntity();

        var productModel = new ProductModel();

        if (productCheck != null)
        {
            productCheck.Count += productAddModel.Count;
            await _productRepository.UpdateAsync(productCheck);

            _logger.LogInformation($"Product with Id {productCheck.Id} added successfully");

            return _mapper.Map<ProductModel>(productCheck);
        }

        productEntity = _mapper.Map<ProductEntity>(productAddModel);
        await _productRepository.AddAsync(productEntity);

        _logger.LogInformation($"Product with Id {productEntity.Id} added successfully");

        return _mapper.Map<ProductModel>(productEntity);
    }

    public async Task<ProductModel?> UpdateAsync(ProductUpdateModel product)
    {
        if (product.Count <= 0)
        {
            throw new NotEnoughProductException("Product count can't be less than 0");
        }

        var productToUpdate = await _productRepository.GetByIdAsync(product.Id);

        productToUpdate.Name = product.Name;
        productToUpdate.Price = product.Price;
        productToUpdate.Manufacturer = product.Manufacturer;
        productToUpdate.Count = product.Count;

        await _productRepository.UpdateAsync(productToUpdate);
        _logger.LogInformation($"Product with Id {product.Id} updated successfully.");

        return _mapper.Map<ProductModel?>(productToUpdate);
    }

    public async Task RemoveAsync(long productId)
    {
        await _productRepository.DeleteAsync(productId);

        _logger.LogInformation($"Product with Id {productId} removed successfully.");
    }
}