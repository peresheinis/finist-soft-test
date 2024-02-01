using Microsoft.IdentityModel.Tokens;

namespace Kernel.Shared.Auth.Interfaces;

public interface IJwtSigningEncodingKey
{
    string SigningAlgorithm { get; }
    SecurityKey GetKey();
}