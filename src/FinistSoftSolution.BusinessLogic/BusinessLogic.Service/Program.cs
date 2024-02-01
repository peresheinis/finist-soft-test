using BusinessLogic.Service.Extensions;
using BusinessLogic.Service.Interceptors;
using BusinessLogic.Service.ProtosServices;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddMapping()
    .AddDatabase()
    .AddInitialUser()
    .AddAuthorization()
    .AddTenantService();

builder.Services.AddGrpc();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<AuthorizationService>();
app.MapGrpcService<AccountsService>();


await app.UseMigrationsAsync();
await app.SeedInitialUserAsync();
await app.RunAsync();
