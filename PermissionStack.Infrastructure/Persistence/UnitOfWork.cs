using PermissionStack.Domain.Entities;
using PermissionStack.Domain.Interfaces;
using PermissionStack.Infrastructure.Repositories;

namespace PermissionStack.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<Permission> Permissions { get; }
        public IRepository<PermissionType> PermissionTypes { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Permissions = new Repository<Permission>(_context);
            PermissionTypes = new Repository<PermissionType>(_context);
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}

