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

app.UseCors(options =>
{
    options
       .WithOrigins("http://localhost:5173")
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowCredentials();
});

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<AuthorizationHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
