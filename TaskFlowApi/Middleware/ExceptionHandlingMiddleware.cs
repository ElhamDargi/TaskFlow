namespace TaskFlowApi.Middleware;

using System.Text.Json;
using TaskFlowApi.DTOs;
using TaskFlowApi.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = exception switch
        {
            NotFoundException notFound => (
                StatusCodes.Status404NotFound, 
                notFound.Message, 
                null
            ),
            ValidationException valEx => (
                StatusCodes.Status400BadRequest, 
                valEx.Message, 
                new Dictionary<string, List<string>> { [valEx.Field] = valEx.Errors }
            ),
            _ => (
                StatusCodes.Status500InternalServerError, 
                "Unexpected Error", 
                null
            )
        };

        context.Response.StatusCode = statusCode;

        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}