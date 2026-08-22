using GymManagement.BLL.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace GymManagement.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is BusinessRuleException businessRuleException)
            {
                _logger.LogWarning(
                    "Business rule rejected on {Path}: {Message}",
                    httpContext.Request.Path,
                    businessRuleException.Message);

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(
                    new { message = businessRuleException.Message },
                    cancellationToken);

                return true;
            }

            _logger.LogError(exception, "An unhandled exception occurred while processing {Path}", httpContext.Request.Path);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(
                new { message = "An unexpected error occurred. Please try again later." },
                cancellationToken);

            return true;
        }
    }
}
