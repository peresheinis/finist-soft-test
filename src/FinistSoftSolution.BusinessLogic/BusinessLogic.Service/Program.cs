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

app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<AuthorizationService>();
app.MapGrpcService<AccountsService>();

await app.UseMigrationsAsync();
await app.SeedInitialUserAsync();
await app.RunAsync();
