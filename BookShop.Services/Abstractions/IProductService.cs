﻿using BookShop.Services.Helper;
using BookShop.Services.Models.PageModels;
using BookShop.Services.Models.ProductModels;

namespace BookShop.Services.Abstractions;

public interface IProductService
{
    Task<PagedList<ProductModel?>> GetAllAsync(ProductPageModel productPageModel);
    Task<ProductModel?> GetByIdAsync(long productId);
    Task<ProductModel?> AddAsync(ProductAddModel productEntity);
    Task<ProductModel?> UpdateAsync(ProductUpdateModel productEntity);
    Task RemoveAsync(long productId);
}
