using BusinessLogic.Service.Extensions;
using BusinessLogic.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddMapping()
    .AddDatabase()
    .AddInitialUser()
    .AddAuthorization();

builder.Services.AddGrpc();

var app = builder.Build();

// Тут нужно добавить Middleware или Interceptor для сервиса,
// который ловит ошибки, как внутренние так и говорящие пользователю
// о его не валидных действиях и возвращает ответ в ErrorDetails 

app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<AuthorizationService>();
app.MapGrpcService<AccountsService>();

await app.UseMigrationsAsync();
await app.SeedInitialUserAsync();
await app.RunAsync();
