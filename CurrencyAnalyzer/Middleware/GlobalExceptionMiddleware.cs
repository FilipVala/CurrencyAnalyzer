using System.Net;
using System.Text.Json;
using CurrencyAnalyzer.Core.Exceptions;

namespace CurrencyAnalyzer.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException ex)
        {
            _logger.LogWarning(ex, "API error occurred");

            await HandleExceptionAsync(
                context,
                HttpStatusCode.BadGateway,
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error occurred");

            await HandleExceptionAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Unexpected server error");
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = message,
            status = (int)statusCode
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}