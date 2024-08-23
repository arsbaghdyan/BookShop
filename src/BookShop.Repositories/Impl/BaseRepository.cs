using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories.Impl
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly BookShopDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(BookShopDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            return _dbSet.AsQueryable();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
