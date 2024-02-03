using Gateway.API.Extensions;
using Gateway.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddBaseConfiguration()
    .AddAuthorization()
    .AddCookiesService()
    .AddHeadersService()
    .AddGrpcClients();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<AuthorizationHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
