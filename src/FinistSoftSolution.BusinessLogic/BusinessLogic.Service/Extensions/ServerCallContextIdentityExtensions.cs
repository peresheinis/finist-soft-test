using Grpc.Core;
using System.Security.Claims;

namespace BusinessLogic.Service.Extensions;

internal static class ServerCallContextIdentityExtensions
{
    /// <summary>
    /// Получить идентификатор авторизованного пользователя
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Guid GetUserId(this ServerCallContext context)
    {
        var httpContext = context
            .GetHttpContext();

        var userClaimsId = httpContext.User.Claims
            .First(e => e.Type == ClaimTypes.NameIdentifier);

        var userId = Guid
            .Parse(userClaimsId.Value);

        return userId;
    }
}