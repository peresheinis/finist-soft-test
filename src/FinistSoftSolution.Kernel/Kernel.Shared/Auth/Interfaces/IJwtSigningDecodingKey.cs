using Microsoft.IdentityModel.Tokens;

namespace Kernel.Shared.Auth.Interfaces;

// Ключ для проверки подписи (публичный)
public interface IJwtSigningDecodingKey
{
    SecurityKey GetKey();
}
