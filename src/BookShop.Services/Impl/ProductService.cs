using AutoMapper;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Exceptions;
using BookShop.Services.Helper;
using BookShop.Services.Models.PageModels;
using BookShop.Services.Models.ProductModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BookShop.Services.Impl;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly IConnectionMultiplexer? _connectionMultiplexer;

    public ProductService(
        IProductRepository productRepository,
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

        if (_connectionMultiplexer?.IsConnected == true)
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

        var productEntities = await _productRepository.GetPagedListAsync(productPageModel);
        var productModels = _mapper.Map<List<ProductModel?>>(productEntities.Items);
        var paginatedProducts = new PagedList<ProductModel?>(productModels, productEntities.TotalCount, productEntities.CurrentPage, productEntities.PageSize);

        if (_connectionMultiplexer?.IsConnected == true)
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
            throw new InvalidProductCountException("Product count can't be less or equal to 0");
        }

        var existingProduct = await _productRepository.GetByDetailsAsync(productAddModel.Manufacturer, productAddModel.Name, productAddModel.Price);

        if (existingProduct != null)
        {
            existingProduct.Count += productAddModel.Count;
            await _productRepository.UpdateAsync(existingProduct);
            _logger.LogInformation($"Product with Id {existingProduct.Id} updated successfully.");
            return _mapper.Map<ProductModel>(existingProduct);
        }

        var productEntity = _mapper.Map<ProductEntity>(productAddModel);
        await _productRepository.AddAsync(productEntity);
        _logger.LogInformation($"Product with Id {productEntity.Id} added successfully.");

        return _mapper.Map<ProductModel>(productEntity);
    }

    public async Task<ProductModel?> UpdateAsync(ProductUpdateModel productUpdateModel)
    {
        if (productUpdateModel.Count <= 0)
        {
            throw new NotEnoughProductException("Product count can't be less than 0");
        }

        var productEntity = await _productRepository.GetByIdAsync(productUpdateModel.Id);
        if (productEntity == null)
        {
            throw new Exception("Product not found");
        }

        productEntity.Name = productUpdateModel.Name;
        productEntity.Price = productUpdateModel.Price;
        productEntity.Manufacturer = productUpdateModel.Manufacturer;
        productEntity.Count = productUpdateModel.Count;

        await _productRepository.UpdateAsync(productEntity);
        _logger.LogInformation($"Product with Id {productUpdateModel.Id} updated successfully.");

        return _mapper.Map<ProductModel?>(productEntity);
    }

    public async Task RemoveAsync(long productId)
    {
        await _productRepository.DeleteAsync(productId);
        _logger.LogInformation($"Product with Id {productId} removed successfully.");
    }
}