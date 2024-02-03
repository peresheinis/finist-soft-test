using BusinessLogic.Core;
using BusinessLogic.Core.Repositories;
using BusinessLogic.Infrastructure;
using BusinessLogic.Infrastructure.Repositories;
using BusinessLogic.Infrastructure.Seeds;
using BusinessLogic.Service.Auth.Abstractions;
using BusinessLogic.Service.Auth.Providers;
using BusinessLogic.Service.Configurations;
using BusinessLogic.Service.Mapping;
using BusinessLogic.Service.Providers;
using BusinessLogic.Service.Validators;
using Kernel.Shared.Auth;
using Kernel.Shared.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Serilog;

namespace BusinessLogic.Service.Extensions;

internal static class StartupExtensions
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

    /// <summary>
    /// Добавить авторизацию в приложение
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddWithValidation<AuthorizationSettings, AuthorizationSettingsValidator>(AuthorizationSettings.ConfigurationSection);

        var authorizationSettings = builder.Configuration
            .GetSection(AuthorizationSettings.ConfigurationSection)
            .Get<AuthorizationSettings>();

        if (authorizationSettings is null)
        {
            throw new ArgumentNullException("Please configure AuthorizationSettings!");
        }

        if (string.IsNullOrWhiteSpace(authorizationSettings.Issuer))
        {
            throw new ArgumentNullException("Please configure Issuer in AuthorizationSettings!");
        }

        if (string.IsNullOrWhiteSpace(authorizationSettings.SecurityKey))
        {
            throw new ArgumentNullException("Please configure SecurityKey in AuthorizationSettings!");
        }

        var signingKey = new SigningSymmetricKey(authorizationSettings.SecurityKey);

        builder.Services
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateActor = false,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(0),
                IssuerSigningKey = signingKey.GetKey(),
                ValidIssuer = authorizationSettings.Issuer,
            });

        builder.Services.AddSingleton<IJwtSigningEncodingKey>(signingKey);
        builder.Services.AddSingleton<IJwtSigningDecodingKey>(signingKey);

        builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        builder.Services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();

        return builder;
    }

    /// <summary>
    /// Настроить <see cref="TenantService"/>
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMapping(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(e =>
        {
            e.AddProfile<BankAccountMapperProfile>();
        });

        return builder;
    }

    /// <summary>
    /// Добавить конфигурацию для нулевого пользователя
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddInitialUser(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddWithValidation<InitialUserSettings, InitialUserSettingsValidator>(InitialUserSettings.ConfigurationSection);

        builder.Services
            .AddScoped<InitialUserSeed>();

        return builder;
    }
   
    /// <summary>
    /// Настроить логирование
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        // Можно добавить сервис для хранения логов,
        // я обычно использую Seq

        builder.Host.UseSerilog();

        return builder;
    }

    /// <summary>
    /// Создать нулевого пользователя
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task SeedInitialUserAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialUserSeed = scope.ServiceProvider.GetRequiredService<InitialUserSeed>();

        await initialUserSeed.ExecuteAsync();
    }

    /// <summary>
    /// Выполнить миграции БД
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task UseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        await database.Database.MigrateAsync();
    }
}
