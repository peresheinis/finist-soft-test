namespace Gateway.API.Services;

/// <summary>
/// Интерфейс для работы с заголовками запроса
/// </summary>
public interface IHeadersService
{
    /// <summary>
    /// Получить Jwt - токен из запроса клиента
    /// </summary>
    /// <returns></returns>
    public string? GetAuthorizationHeaderValue();
}
