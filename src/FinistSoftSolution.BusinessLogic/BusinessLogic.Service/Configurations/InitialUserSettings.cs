namespace BusinessLogic.Service.Configurations;

public class InitialUserSettings
{
    /// <summary>
    /// Название секции конфигурации
    /// </summary>
    public const string ConfigurationSection = "InitialUser";
    /// <summary>
    /// Номер телефона начального пользователя
    /// </summary>
    public string PhoneNumber { get; set; } = null!;
    /// <summary>
    /// Пароль начального пользователя
    /// </summary>
    public string Password { get; set; } = null!;
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string FirstName { get; set; } = null!;
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string LastName { get; set; } = null!;
    /// <summary>
    /// Отчество пользователя
    /// </summary>
    public string? MiddleName{ get; set; }
}
