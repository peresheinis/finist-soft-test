using System.Security.Claims;

namespace Gateway.API.Extensions;

public static class HttpContextExtensions
{
    public static string GetUserFullName(this HttpContext httpContext) =>
        httpContext.User.Claims.First(e => e.Type == ClaimTypes.GivenName).Value;
}
