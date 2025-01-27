using EShop.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Action<HttpContext> _executeBeforeHandler;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private const string _logTemplate = "{ErrorMessage} from {UserName}";

        public ExceptionHandlerMiddleware(RequestDelegate next, Action<HttpContext> executeBeforeHandler, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _executeBeforeHandler = executeBeforeHandler;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {          
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _executeBeforeHandler(context);

                Task handle = exception switch
                {
                    NotFoundException ex => HandleNotFoundExceptionAsync(ex, context),
                    BusinessException ex => HandleBusinessExceptionAsync(ex, context),
                    AuthenticationException ex => HandleAuthenticationExceptionAsync(ex, context),
                    AuthorizationException ex => HandleAuthorizationExceptionAsync(ex, context),
                    Exception ex => HandleExceptionAsync(ex, context)
                };
                await handle;
            }
        }

        private async Task HandleNotFoundExceptionAsync(NotFoundException ex, HttpContext context)
        {
            _logger.LogInformation(_logTemplate, ex.Message, context.User.Identity?.Name ?? "Unknown");

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Not found",
                Detail = ex.Message
            });
        }

        private async Task HandleBusinessExceptionAsync(BusinessException ex, HttpContext context)
        {
            _logger.LogInformation(_logTemplate, ex.Message, context.User.Identity?.Name ?? "Unknown");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Rule breached",
                Detail = ex.Message
            });
        }

        private async Task HandleAuthenticationExceptionAsync(AuthenticationException ex, HttpContext context)
        {
            _logger.LogWarning(_logTemplate, ex.Message, context.User.Identity?.Name ?? "Unknown");

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Authentication error",
                Detail = ex.Message
            });
        }

        private async Task HandleAuthorizationExceptionAsync(AuthorizationException ex, HttpContext context)
        {
            _logger.LogWarning(_logTemplate, ex.Message, context.User.Identity?.Name ?? "Unknown");

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Access denied",
                Detail = ex.Message
            });
        }

        private async Task HandleExceptionAsync(Exception ex, HttpContext context)
        {
            _logger.LogError(ex, _logTemplate, ex.Message, context.User.Identity?.Name ?? "Unknown");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Server error",
                Detail = ex.Message
            });
        }
    }
}
