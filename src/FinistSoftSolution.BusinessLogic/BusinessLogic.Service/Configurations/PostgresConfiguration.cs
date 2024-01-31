namespace BusinessLogic.Service.Configurations;

internal sealed class PostgresConfiguration
{
    /// <summary>
    /// Название конфигурации
    /// </summary>
    public const string ConfigurationSection = "PostgresDb";
    /// <summary>
    /// Хост до БД
    /// </summary>
    public string Host { get; set; } = null!;
    /// <summary>
    /// Порт до БД
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// Имя БД
    /// </summary>
    public string Database { get; set; } = null!;
    /// <summary>
    /// Пользователь БД
    /// </summary>
    public string User { get; set; } = null!;
    /// <summary>
    /// Пароль до БД
    /// </summary>
    public string Password { get; set; } = null!;
}
