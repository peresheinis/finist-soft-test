using BusinessLogic.Core.Entities;
using BusinessLogic.Service.Auth.Abstractions;
using BusinessLogic.Service.Configurations;
using Kernel.Shared.Auth.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BusinessLogic.Service.Auth.Providers;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly IJwtSigningEncodingKey _jwtSigningEncodingKey;
    private readonly IOptionsMonitor<AuthorizationSettings> _jwtOptions;

    public JwtTokenProvider(
        IOptionsMonitor<AuthorizationSettings> jwtOptions,
        IJwtSigningEncodingKey jwtSigningEncodingKey)
    {
        _jwtOptions = jwtOptions;
        _jwtSigningEncodingKey = jwtSigningEncodingKey;
    }

    /// <summary>
    /// Сгенерировать Jwt токен для авторизации клиента
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string? CreateToken(User user)
    {
        var identity = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64),
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
                new(ClaimTypes.GivenName, user.FullName.ToString())
            });

        var currentJwtsettings = _jwtOptions.CurrentValue;

        var credentials = new SigningCredentials(
            _jwtSigningEncodingKey.GetKey(),
            _jwtSigningEncodingKey.SigningAlgorithm);

        var jwtToken = new JwtSecurityToken(
            currentJwtsettings.Issuer,
            claims: identity.Claims,
            expires: DateTime.UtcNow.AddMinutes(currentJwtsettings.ExpiresInMinutes),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwtToken);

        return token;
    }

    private static long ToUnixEpochDate(DateTime date)
    {
        return new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }
}
