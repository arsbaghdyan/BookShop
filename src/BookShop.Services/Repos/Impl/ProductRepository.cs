using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Repositories.Impl;
using BookShop.Services.Helper;
using BookShop.Services.Models.PageModels;
using BookShop.Services.Repos.Inetrfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookShop.Services.Repos.Impl
{
    public class ProductRepository : Repository<ProductEntity>, IProductRepository
    {
        public ProductRepository(BookShopDbContext context) : base(context) { }

        public async Task<ProductEntity?> GetByIdAsync(long productId)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<ProductEntity?> GetByDetailsAsync(string manufacturer, string name, decimal price)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Manufacturer == manufacturer && p.Name == name && p.Price == price);
        }

        public async Task<PagedList<ProductEntity>> GetPagedListAsync(ProductPageModel productPageModel)
        {
            IQueryable<ProductEntity> query = _dbSet;

            Expression<Func<ProductEntity, object>> keySelector = productPageModel.OrderBy?.ToLower() switch
            {
                "id" => p => p.Id,
                "name" => p => p.Name,
                "price" => p => p.Price,
                "manufacturer" => p => p.Manufacturer,
                "count" => p => p.Count,
                _ => p => p.Name,
            };

            query = productPageModel.IsOrderAsc ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);

            return await PagedList<ProductEntity>.ToPagedListAsync(query, productPageModel.PageNumber, productPageModel.PageSize);
        }
    }
}
