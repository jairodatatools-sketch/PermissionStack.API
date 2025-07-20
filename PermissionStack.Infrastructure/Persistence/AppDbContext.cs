using Microsoft.EntityFrameworkCore;
using PermissionStack.Domain.Entities;

namespace PermissionStack.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
    }
}

