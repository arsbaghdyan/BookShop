using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IProductService
{
    Task<List<ProductModel>> GetAllAsync();
    Task<ProductModel> GetByIdAsync(long productId);
    Task<ProductModel> AddAsync(ProductAddModel productEntity);
    Task<ProductModel> UpdateAsync(ProductUpdateModel productEntity);
    Task RemoveAsync(long productId);
    Task ClearAsync();
}
