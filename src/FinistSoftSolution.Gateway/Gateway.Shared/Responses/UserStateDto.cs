namespace Gateway.Shared.Responses;

/// <summary>
/// Информация о пользователе
/// </summary>
/// <param name="FullName">Полное имя пользователя</param>
public record UserStateDto(string FullName); 
// Сюда можно добавить по идее любые данные из Jwt - токена
// Можно эти же данные получать и без запроса на фронтенде,
// просто десериализуя Jwt - токен