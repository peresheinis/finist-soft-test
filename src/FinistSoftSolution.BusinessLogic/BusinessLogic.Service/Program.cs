using BusinesLogic.Service.Services;
using BusinessLogic.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddDatabase()
    .AddAuthorization()
    .AddInitialUser();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<AuthorizationService>();


await app.UseMigrationsAsync();
await app.SeedInitialUserAsync();
await app.RunAsync();
