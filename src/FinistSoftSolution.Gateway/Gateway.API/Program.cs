using Gateway.API.Configurations;
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
    var hosts = app.Configuration
        .GetSection(HostsSettings.ConfigurationSection)
        .Get<HostsSettings>() ?? throw new ArgumentNullException(nameof(HostsSettings));
        
    options
       .WithOrigins(hosts.FrontendHost)
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
