using BookShop.Data.Entities;

namespace BookShop.Services.Abstractions;

public interface IProductService
{
    Task AddAsync(ProductEntity productEntity);
    Task RemoveAsync(long productId);
    Task<List<ProductEntity>> GetAllAsync();
    Task<ProductEntity> GetByIdAsync(long productId);
    Task<ProductEntity> UpdateAsync(ProductEntity productEntity);
    Task ClearAsync();
}
