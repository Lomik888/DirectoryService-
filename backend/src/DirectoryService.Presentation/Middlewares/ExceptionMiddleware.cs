using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using DirectoryService.Presentation.Responses;

namespace DirectoryService.Presentation.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await ExceptionHandlerAsync(context, ex);
        }
    }

    private async Task ExceptionHandlerAsync(HttpContext context, Exception exception)
    {
        var (error, statusCode) = GetErrorWithStatusCode(exception);
        var envelope = Envelope<Errors>.Error(Errors.Create(error.ToList()));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        _logger.LogError(exception, exception.Message);

        await context.Response.WriteAsJsonAsync(envelope);
    }

    private (Error, int) GetErrorWithStatusCode(Exception ex)
    {
        return ex switch
        {
            Microsoft.EntityFrameworkCore.DbUpdateException =>
                (Error.Create(ex.Message, "db.update.exception", ErrorTypes.DB),
                    StatusCodes.Status500InternalServerError),
            _ =>
                (Error.Create("Неизвестная ошибка", "unknown.error", ErrorTypes.UNKNOWN),
                    StatusCodes.Status500InternalServerError)
        };
    }
}