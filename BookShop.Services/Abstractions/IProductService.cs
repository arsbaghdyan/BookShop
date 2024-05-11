using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Abstractions;

public interface IProductService
{
    Task AddAsync(ProductAddVm productEntity);
    Task RemoveAsync(long productId);
    Task<List<ProductGetVm>> GetAllAsync();
    Task<ProductGetVm> GetByIdAsync(long productId);
    Task UpdateAsync(ProductUpdateVm productEntity);
    Task ClearAsync();
}
