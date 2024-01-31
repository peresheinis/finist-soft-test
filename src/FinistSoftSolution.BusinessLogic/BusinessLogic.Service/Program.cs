using BusinesLogic.Service.Services;
using BusinessLogic.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabase();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.UseMigrationsAsync();
await app.RunAsync();
