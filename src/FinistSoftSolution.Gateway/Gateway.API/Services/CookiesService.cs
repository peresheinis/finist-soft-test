namespace Gateway.API.Services;

/// <summary>
/// Сервис для работы с куками
/// </summary>
public class CookiesService : ICookiesService
{
    /// <summary>
    /// Название ключа в куках, в которых лежит Jwt token
    /// </summary>
    public const string AuthorizationCookieKey = "Authorization";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddAuthorizationCookies(HttpContext httpContext, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        { 
            throw new ArgumentNullException(nameof(value));
        }

        // В Production тут нужно установить CookiesOptions =>
        // HttpOnly = true, IsSecure = true, эту конфигурацию можно принимать
        // через DI IOptions<CookiesSettings> { IsHttpOnly, IsSecure }

        httpContext.Response.Cookies
            .Append(AuthorizationCookieKey, value); 
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string? GetAuthorizationCookies(HttpContext httpContext)
    {
        if (!httpContext.Request.Cookies.Any(c => c.Key.Equals(AuthorizationCookieKey)))
        {
            return null;
        }

        var authorizationCookies = httpContext.Request.Cookies
            .First(c => c.Key.Equals(AuthorizationCookieKey));

        return authorizationCookies.Value;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void RemoveAuthorizationCookies(HttpContext httpContext)
    {
        // В Production тут нужно установить CookiesOptions =>
        // HttpOnly = true, IsSecure = true, эту конфигурацию можно принимать
        // через DI IOptions<CookiesSettings> { IsHttpOnly, IsSecure }

        httpContext.Response.Cookies
            .Delete(AuthorizationCookieKey); 
    }
}