namespace Gateway.API.Consts;

/// <summary>
/// Название Grpc клиентов для <see cref="Grpc.Net.ClientFactory.GrpcClientFactory"/>
/// </summary>
public static class GrpcClientsNames
{
    /// <summary>
    /// Клиент до сервиса с банковскими счетами
    /// </summary>
    public const string Accounts = "Accounts";
    /// <summary>
    /// Клиент до сервиса авторизации
    /// </summary>
    public const string Authorization = "Authorization";
}
