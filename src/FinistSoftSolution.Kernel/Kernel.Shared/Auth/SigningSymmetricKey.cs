using Kernel.Shared.Auth.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Kernel.Shared.Auth;

public class SigningSymmetricKey : IJwtSigningEncodingKey, IJwtSigningDecodingKey
{
    private readonly SymmetricSecurityKey _secretKey;

    public SigningSymmetricKey(string key)
    {
        _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }

    public string SigningAlgorithm { get; } = SecurityAlgorithms.HmacSha256;

    public SecurityKey GetKey()
    {
        return _secretKey;
    }
}
