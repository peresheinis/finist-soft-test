namespace Gateway.API.Configurations;

/// <summary>
/// Настройки для авторизации пользователя в приложении
/// </summary>
public class AuthorizationSettings
{
    public const string ConfigurationSection = "Authorization";

    /// <summary>
    /// Секретный ключ для подписи Jwt токена
    /// </summary>
    public string SecurityKey { get; set; } = null!;
    /// <summary>
    /// Издатель ключа
    /// </summary>
    public string Issuer { get; set; } = string.Empty;
}
