using BookShop.Data.Entities;
using BookShop.Repositories.Interfaces;
using BookShop.Services.Helper;
using BookShop.Services.Models.PageModels;

namespace BookShop.Services.Repos.Impl
{
    public interface IProductRepository : IRepository<ProductEntity>
    {
        Task<ProductEntity?> GetByIdAsync(long productId);
        Task<ProductEntity?> GetByDetailsAsync(string manufacturer, string name, decimal price);
        Task<PagedList<ProductEntity>> GetPagedListAsync(ProductPageModel productPageModel);
    }
}
