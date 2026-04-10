using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using TodoServices.Exceptions;

namespace TodoApi.Middleware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            _logger.LogError(exception, "An unhandled exception has occurred with traceId: {traceId}", traceId);
            _logger.LogInformation("Mapping exception to HTTP response with traceId: {traceId}", traceId);

            var (statusCode, title) = MapException(exception);

            await Results.Problem(
                title: title,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?>
                {
                    {"traceId", traceId}
                }
                ).ExecuteAsync(httpContext);

            return true;
        }

        private static (int statusCode, string title) MapException(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                MissingDataException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An error occurred")
            };
        }
    }
}
