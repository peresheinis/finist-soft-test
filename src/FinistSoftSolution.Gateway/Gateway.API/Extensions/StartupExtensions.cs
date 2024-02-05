using Gateway.API.Configurations;
using Gateway.API.Consts;
using Gateway.API.Protos;
using Gateway.API.Services;
using Grpc.Core;
using Kernel.Shared.Auth;
using Kernel.Shared.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.API.Extensions;

public static class StartupExtensions
{
    /// <summary>
    /// Выполнить базовую настройку Web API
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddBaseConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    /// <summary>
    /// Добавить клиентов до Grpc сервисов
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddGrpcClients(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddGrpcClient<Authorization.AuthorizationClient>(GrpcClientsNames.Authorization, o =>
            {
                // Можно добавить валидацию на конфигурацию
                // И Выкидывать говорящую ошибку, если конфигурация
                // хоста не соответствует
                o.Address = new Uri(builder.Configuration
                    .GetRequiredSection(HostsSettings.ConfigurationSection)
                    .Get<HostsSettings>().BusinessLogicServiceHost);
            })
            .AddCallCredentials(CallCredentials)
            .ConfigureChannel(channel => channel.UnsafeUseInsecureChannelCallCredentials = true);

        builder.Services
            .AddGrpcClient<Accounts.AccountsClient>(GrpcClientsNames.Accounts, o =>
            {
                // Можно добавить валидацию на конфигурацию
                // И Выкидывать говорящую ошибку, если конфигурация
                // хоста не соответствует
                o.Address = new Uri(builder.Configuration
                    .GetRequiredSection(HostsSettings.ConfigurationSection)
                    .Get<HostsSettings>().BusinessLogicServiceHost);
            })
            .AddCallCredentials(CallCredentials)
            .ConfigureChannel(channel => channel.UnsafeUseInsecureChannelCallCredentials = true);

        return builder;

        Task CallCredentials(AuthInterceptorContext context, Metadata entries, IServiceProvider serviceProvider)
        {
            var headersService = serviceProvider.GetRequiredService<IHeadersService>();
            var authorizationToken = headersService.GetAuthorizationHeaderValue();

            if (authorizationToken != null)
            {
                entries.Add("Authorization", authorizationToken);
            }

            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Добавить сервис для работы с куками запроса
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddCookiesService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICookiesService, CookiesService>();
        builder.Services.AddScoped<ICookiesReadService>(_ => _.GetRequiredService<ICookiesService>());
        builder.Services.AddScoped<ICookiesWriteService>(_ => _.GetRequiredService<ICookiesService>());

        return builder;
    }

    /// <summary>
    /// Добавить сервис для работы с заголовками запроса
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddHeadersService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IHeadersService, HeadersService>();

        return builder;
    }

    /// <summary>
    /// Добавить авторизацию
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
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
                IssuerSigningKey = signingKey.GetKey(),
                ValidIssuer = authorizationSettings.Issuer,
            });

        builder.Services.AddSingleton<IJwtSigningEncodingKey>(signingKey);
        builder.Services.AddSingleton<IJwtSigningDecodingKey>(signingKey);

        return builder;
    }
}
