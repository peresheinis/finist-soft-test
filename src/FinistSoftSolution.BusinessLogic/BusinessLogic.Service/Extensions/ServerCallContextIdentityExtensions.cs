using Grpc.Core;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogic.Service.Extensions;

internal static class ServerCallContextIdentityExtensions
{
    public static Guid GetUserId(this ServerCallContext context) => Guid
        .Parse(context.GetHttpContext().User.Claims.First(e => e.Type == JwtRegisteredClaimNames.Sub).Value);
}