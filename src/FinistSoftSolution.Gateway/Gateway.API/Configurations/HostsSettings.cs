namespace Gateway.API.Configurations;

/// <summary>
/// Конфигурация хостов до сервисов
/// </summary>
public class HostsSettings
{
    public const string ConfigurationSection = "Hosts";

    /// <summary>
    /// Хост до сервиса BusinessLogic
    /// </summary>
    public string BusinessLogicServiceHost { get; set; } = null!;

    /// <summary>
    /// Хост фронтенда
    /// </summary>
    public string FrontendHost { get; set; } = null!;
}