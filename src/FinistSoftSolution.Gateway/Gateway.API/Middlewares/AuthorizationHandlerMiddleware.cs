using Gateway.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;

namespace Gateway.API.Middlewares;

/// <summary>
/// Посредник для получения токена и кук и 
/// установки их в Request.Headers.Authorization
/// </summary>
public class AuthorizationHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public AuthorizationHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var readCookiesService = httpContext.RequestServices
            .GetRequiredService<ICookiesReadService>();

        var authorizationToken = readCookiesService
            .GetAuthorizationCookies(httpContext);

        if (!string.IsNullOrWhiteSpace(authorizationToken))
        {
            httpContext.Request.Headers.Authorization =
                $"{JwtBearerDefaults.AuthenticationScheme} " +
                $"{authorizationToken}";
        }

        await _next(httpContext);
    }
}
