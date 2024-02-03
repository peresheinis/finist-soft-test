using Grpc.Core;
using Kernel.Shared.Errors;
using System.Net;

namespace Gateway.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (RpcException rpcException)
        {
            var (errorMessage, statusCode) = rpcException.Status.StatusCode switch
            {
                StatusCode.NotFound => ("Запрашиваемый ресур не найден", HttpStatusCode.NotFound),
                StatusCode.Internal => ("Внутренняя ошибка сервиса", HttpStatusCode.InternalServerError),
                StatusCode.InvalidArgument => ("Ошибка запроса", HttpStatusCode.BadRequest),
                StatusCode.AlreadyExists => ("Сущность уже существует", HttpStatusCode.Conflict),
                StatusCode.PermissionDenied => ("Доступ запрещен", HttpStatusCode.Forbidden),
                StatusCode.Cancelled => ("Превышен интервал ожидания запроса", HttpStatusCode.RequestTimeout),
                StatusCode.Unauthenticated => ("Доступ запрещен", HttpStatusCode.Unauthorized),
                _ => ("Неизвестная ошибка", HttpStatusCode.InternalServerError)
            };

            var errorResponse = new ErrorResponse
            {
                Message = errorMessage,
                Details = rpcException.Status.Detail,
                RequestId = httpContext.TraceIdentifier
            };

            httpContext.Response.StatusCode = (int)statusCode;

            await httpContext.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception ex)
        {
            var errorResponse = new ErrorResponse
            {
                Message = "Упс... Произошла непредвиденная ошибка...",
                Details = "Мы уже разбираемся с проблемой.",
                RequestId = httpContext.TraceIdentifier
            };

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
