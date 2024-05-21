using AutoMapper;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Helper;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.PageModels;
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

    public async Task<PagedList<ProductModel>> GetAllAsync(ProductPageModel productPageModel)
    {
        IQueryable<ProductEntity> productQuery = _bookShopDbContext.Products;

        if (!string.IsNullOrEmpty(productPageModel.OrderBy))
        {
            switch (productPageModel.OrderBy.ToLower())
            {
                case "id":
                    productQuery = productPageModel.OrderDirection.ToLower() == "asc"
                        ? productQuery.OrderBy(p => p.Id)
                        : productQuery.OrderByDescending(p => p.Id);
                    break;
                case "name":
                    productQuery = productPageModel.OrderDirection.ToLower() == "asc"
                        ? productQuery.OrderBy(p => p.Name)
                        : productQuery.OrderByDescending(p => p.Name);
                    break;
                case "price":
                    productQuery = productPageModel.OrderDirection.ToLower() == "asc"
                        ? productQuery.OrderBy(p => p.Price)
                        : productQuery.OrderByDescending(p => p.Price);
                    break;
                case "manufacturer":
                    productQuery = productPageModel.OrderDirection.ToLower() == "asc"
                        ? productQuery.OrderBy(p => p.Manufacturer)
                        : productQuery.OrderByDescending(p => p.Manufacturer);
                    break;
                case "count":
                    productQuery = productPageModel.OrderDirection.ToLower() == "asc"
                        ? productQuery.OrderBy(p => p.Count)
                        : productQuery.OrderByDescending(p => p.Count);
                    break;
                default:
                    productQuery = productQuery.OrderByDescending(p => p.Name);
                    break;
            }
        }

        var productEntities = await PagedList<ProductEntity>
            .ToPagedListAsync(productQuery, productPageModel.PageNumber, productPageModel.PageSize);

        var productModels = _mapper.Map<List<ProductModel>>(productEntities.Items);

        return new PagedList<ProductModel>(productModels, productEntities.TotalCount, productEntities.CurrentPage, productEntities.PageSize);
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
         && p.Name == productAddModel.Name && p.Price == productAddModel.Price);

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
        productToUpdate.Count = product.Count;

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with Id {product.Id} updated successfully.");

        var productModel = _mapper.Map<ProductModel>(productToUpdate);

        return productModel;
    }

    public async Task RemoveAsync(long productId)
    {
        await _bookShopDbContext.Products.Where(p => p.Id == productId).ExecuteDeleteAsync();

        _logger.LogInformation($"Product with Id {productId} removed successfully.");
    }
}
