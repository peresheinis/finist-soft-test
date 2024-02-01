using BusinessLogic.Core.Entities;

namespace BusinessLogic.Service.Auth.Abstractions;

/// <summary>
/// Интерфейс Jwt провайдера для генерации Jwt токенов
/// </summary>
public interface IJwtTokenProvider
{
    /// <summary>
    /// Сгенерировать Jwt токен для аккаунта клиента
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string? CreateToken(User user);
}