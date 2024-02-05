using System.Security.Claims;

namespace Gateway.API.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    /// Получить полное имя пользователя
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string GetUserFullName(this HttpContext httpContext) =>
        httpContext.User.Claims.First(e => e.Type == ClaimTypes.GivenName).Value;
}
