namespace BusinessLogic.Service.Configurations;

public class AuthorizationSettings
{
    public const string ConfigurationSection = "Authorization";

    public string SecurityKey { get; set; } = null!;
    public string Issuer { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; } = 5;
}
