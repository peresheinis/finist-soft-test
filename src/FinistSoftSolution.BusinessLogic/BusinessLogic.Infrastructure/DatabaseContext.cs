using BusinessLogic.Core.Entities;
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
        // ToDo: добавить конфигурацию 
    }
}
