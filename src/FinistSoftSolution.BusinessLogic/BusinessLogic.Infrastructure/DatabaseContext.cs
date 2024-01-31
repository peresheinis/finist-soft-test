using BusinessLogic.Core.Entities;
using BusinessLogic.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    /// <summary>
    /// Пользователи сервиса
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Банковские счета пользователей сервиса
    /// </summary>
    public DbSet<BankAccount> BankAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
