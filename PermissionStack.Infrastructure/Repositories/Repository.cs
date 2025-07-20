using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PermissionStack.Domain.Interfaces;

namespace PermissionStack.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
            => await _entities.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _entities.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _entities.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity)
            => await _entities.AddAsync(entity);

        public void Update(T entity)
            => _entities.Update(entity);

        public void Remove(T entity)
            => _entities.Remove(entity);
    }
}

