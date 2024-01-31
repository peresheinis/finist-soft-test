namespace BusinessLogic.Service.Providers;

/// <summary>
/// Провайдер для хеширования и валидации пароля
/// </summary>
public class PasswordHashProvider : IPasswordHashProvider
{
    public Task<string> HashAsync(string password, CancellationToken cancellationToken = default)
    {
        // Выполнить хеш пароля
        // Необходимо хешировать пароль, для того что бы не было
        // утечек оригинала пароля в случае компроментации бд
        return Task.FromResult(password);
    }

    public Task<bool> ValidateAsync(string password, string passwordHash, CancellationToken cancellationToken = default)
    {
        // Заглушка на валидацию пароля по хешу 
        // Необходимо валидировать пароль по хешу, иначе никак не сравнить
        // В основном я использую Microsoft.AspNet.Identity.Core,
        // вместо того варианта что есть сейчас
        return Task.FromResult(password == passwordHash);
    }
}