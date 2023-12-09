using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.Repository;

namespace Vezeeta.Repository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().CountAsync(match);
        }

        public bool Delete(int id)
        {
            T? entity = _context.Set<T>().Find(id);

            if (entity == null) return false;

            _context.Set<T>().Remove(entity);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            T? entity = await _context.Set<T>().FindAsync(id);

            if (entity == null) return false;

            _context.Set<T>().Remove(entity);

            return true;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, int page, int pageSize, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>().Where(match);

            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            var entities = await query.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            return entities;
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> match, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            var entity = await query.SingleOrDefaultAsync(match);

            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().AnyAsync(match);
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);

            return entity;
        }
    }
}
