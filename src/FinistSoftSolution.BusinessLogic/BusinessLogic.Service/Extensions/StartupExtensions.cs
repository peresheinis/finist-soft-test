using BusinessLogic.Core;
using BusinessLogic.Core.Repositories;
using BusinessLogic.Infrastructure;
using BusinessLogic.Infrastructure.Repositories;
using BusinessLogic.Service.Configurations;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BusinessLogic.Service.Extensions;

public static class StartupExtensions
{
    /// <summary>
    /// Выполнить настройку БД
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        var postgresConfiguration = builder.Configuration
            .GetSection(PostgresConfiguration.ConfigurationSection)
            .Get<PostgresConfiguration>();

        if (postgresConfiguration is null)
        {
            throw new ArgumentNullException(nameof(postgresConfiguration),
                "Please configure your database connection.");
        }

        if (string.IsNullOrEmpty(postgresConfiguration.User))
        {
            throw new ArgumentNullException(nameof(postgresConfiguration.User), 
                "Please configure your database user.");
        }

        if (string.IsNullOrEmpty(postgresConfiguration.Password))
        {
            throw new ArgumentNullException(nameof(postgresConfiguration.Password), 
                "Please configure your database password.");
        }

        if (string.IsNullOrEmpty(postgresConfiguration.Database))
        {
            throw new ArgumentNullException(nameof(postgresConfiguration.Database), 
                "Please configure your database name.");
        }

        if (string.IsNullOrEmpty(postgresConfiguration.Host))
        {
            throw new ArgumentNullException(nameof(postgresConfiguration.Host), 
                "Please configure your database host.");
        }

        if (postgresConfiguration.Port is default(int))
        {
            throw new ArgumentNullException(nameof(postgresConfiguration.Host), 
                "Please configure your database port.");
        }

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder();

        connectionStringBuilder.Host = postgresConfiguration.Host;
        connectionStringBuilder.Port = postgresConfiguration.Port;
        connectionStringBuilder.Username = postgresConfiguration.User;
        connectionStringBuilder.Password = postgresConfiguration.Password;
        connectionStringBuilder.Database = postgresConfiguration.Database;

        // В идеале на чтение и на запись в бд должно быть 2 разных пользователя

        builder.Services.AddDbContext<DatabaseContext>(
            options => options.UseNpgsql(connectionStringBuilder.ConnectionString));

        builder.Services.AddScoped<IUnitOfWork, UnitOfWorkPostgres>();
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IBankAccountsRepository, BankAccountsRepository>();

        return builder;
    }

    // public static WebApplicationBuilder AddLogging(this WebApplicationBuilder webApplicationBuilder) { }
    // Можно добавить логирования, в основном я использую Serilog + Seq

    /// <summary>
    /// Выполнить миграции БД
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static Task UseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        return database.Database.MigrateAsync();
    }
}
