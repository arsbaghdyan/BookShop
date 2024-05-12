using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IProductService
{
    Task AddAsync(ProductAddModel productEntity);
    Task RemoveAsync(long productId);
    Task<List<ProductModel>> GetAllAsync(long productId);
    Task<ProductModel> GetByIdAsync(long productId);
    Task UpdateAsync(ProductUpdateModel productEntity);
    Task ClearAsync();
}
