using Microsoft.EntityFrameworkCore;
using SmartCookbook.Domain.Entities;

namespace SmartCookbook.Infrastructure.RepositoryAccess;

public class SmartCookbookContext : DbContext
{
    public SmartCookbookContext(DbContextOptions<SmartCookbookContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartCookbookContext).Assembly);
    }
}
