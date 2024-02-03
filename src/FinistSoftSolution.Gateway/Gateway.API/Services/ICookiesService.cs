namespace Gateway.API.Services;

/// <summary>
/// Интерфейс для работы с куками пользователя
/// </summary>
public interface ICookiesService : ICookiesReadService, ICookiesWriteService
{
}

/// <summary>
/// Интерфейс для записи кук в ответ
/// </summary>
public interface ICookiesWriteService
{
    /// <summary>
    /// Добавить Jwt Токен в куки
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="value">Jwt Token</param>
    public void AddAuthorizationCookies(HttpContext httpContext, string value);
    /// <summary>
    /// Удалить Jwt Токен из кук
    /// </summary>
    /// <param name="httpContext"></param>
    public void RemoveAuthorizationCookies(HttpContext httpContext);
}

/// <summary>
/// Интерфейс для чтения кук из запроса
/// </summary>
public interface ICookiesReadService
{ 
    /// <summary>
    /// Получить Jwt Токен из кук
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public string? GetAuthorizationCookies(HttpContext httpContext);
}