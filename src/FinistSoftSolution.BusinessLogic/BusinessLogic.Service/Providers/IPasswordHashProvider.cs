namespace BusinessLogic.Service.Providers;

/// <summary>
/// Провайдер хеширования пароля
/// </summary>
public interface IPasswordHashProvider
{
    /// <summary>
    /// Захешировать пароль
    /// </summary>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> HashAsync(string password, CancellationToken cancellationToken = default);
    /// <summary>
    /// Выполнить валидацию пароль по хешу
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordHash"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> ValidateAsync(string password, string passwordHash, CancellationToken cancellationToken = default);
}
