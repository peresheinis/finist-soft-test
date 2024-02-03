namespace Gateway.API.Configurations;

public class AuthorizationSettings
{
    public const string ConfigurationSection = "Authorization";

    public string SecurityKey { get; set; } = null!;
    public string Issuer { get; set; } = string.Empty;
}
