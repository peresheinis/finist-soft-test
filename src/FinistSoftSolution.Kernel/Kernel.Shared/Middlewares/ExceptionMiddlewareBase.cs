using Kernel.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Net;
using System.Text.Json;

namespace Kernel.Shared.Middlewares;

public class ExceptionMiddlewareBase
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _hostEnvironment;

    public ExceptionMiddlewareBase(ILogger logger, RequestDelegate next, IHostEnvironment hostEnvironment)
    {
        _next = next;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AlertingException alertingException)
        {
            await HandleAlertingException(context, alertingException);
        }
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }

    public virtual async Task HandleException(HttpContext context, Exception ex)
    {
        var (exMessage, userMessage, detail, statusCode) = ex switch
        {
            TaskCanceledException _ => (
                "Timeout",
                "Запрос был отменен",
                "Превышение допустимого времени выполнения. Попробуйте еще-раз",
                HttpStatusCode.InternalServerError),

            _ => ComposeDefaultMessage(ex)
        };

        _logger.LogCritical(ex, exMessage);

        await SendErrorResponse(context, ErrorApiResponseHelper.Error(context, "Unhandled exception", userMessage, detail), (int)statusCode);

        (string, string, string, HttpStatusCode HttpStatusCode) ComposeDefaultMessage(Exception ex)
        {
            if (_hostEnvironment.IsProduction())
            {
                return (
                    "Unhandled exception",
                    "Что-то пошло не так.",
                    "Мы уже разбираемся с проблемой", HttpStatusCode.InternalServerError);
            }

            return ("Unhandled exception", ex.Message, ex.ToString(), HttpStatusCode.InternalServerError);
        }
    }

    public virtual async Task HandleAlertingException(HttpContext context, AlertingException ex)
    {
        var statusCode = ex switch
        {
            ConflictException when ex.Code is ExceptionCodes.Conflict => StatusCodes.Status409Conflict,
            ConflictException when ex.Code is ExceptionCodes.EntityNotFound => StatusCodes.Status404NotFound,
            ConflictException when ex.Code is ExceptionCodes.SessionExpired => StatusCodes.Status401Unauthorized,
            ConflictException when ex.Code is ExceptionCodes.ServiceUnavailable => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status400BadRequest
        };

        await SendErrorResponse(context, ErrorApiResponseHelper.Error(context, ex), statusCode);
    }

    private static Task SendErrorResponse(HttpContext context, ErrorResponse apiError, int statusCode)
    {
        var errorMessage = JsonSerializer.Serialize(apiError);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(errorMessage);
    }
}

public static class ErrorApiResponseHelper
{
    public static ErrorResponse Error(HttpContext context, AlertingException exception)
    {
        return Error(context, exception.Code, exception.Message, exception.Details,
            MapExceptionDictionary(exception));
    }

    public static ErrorResponse Error(HttpContext context, string code, string message, string details,
        IDictionary<string, object>? data = null)
    {
        return new ErrorResponse(code, context.TraceIdentifier, message, details, data);
    }

    private static IDictionary<string, object>? MapExceptionDictionary(Exception ex)
    {
        IDictionary<string, object?>? data = null;

        if (ex.Data.Count > 0)
            data = ex.Data
                .Cast<DictionaryEntry>()
                .ToDictionary(e => e.Key.ToString() ?? "unknown",
                              e => e.Value);

        return data;
    }
}
