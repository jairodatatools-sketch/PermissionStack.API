using PermissionStack.Domain.Entities;

namespace PermissionStack.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Permission> Permissions { get; }
        IRepository<PermissionType> PermissionTypes { get; }

        Task<int> SaveChangesAsync();
    }
}

