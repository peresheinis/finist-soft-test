namespace Gateway.API.Services;

/// <summary>
/// Сервис для работы с заголовками запроса пользователя
/// </summary>
public class HeadersService : IHeadersService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HeadersService(IHttpContextAccessor httpContextAccessor)
    { 
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetAuthorizationHeaderValue()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        return httpContext?.Request.Headers.Authorization;
    }
}
