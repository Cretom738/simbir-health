using Application.Dtos;
using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebApi.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ErrorCodeException e)
            {
                await HandleException(context, e);
            }
            catch (Exception e)
            {
                HandleException(context, e);
            }
        }

        private async Task HandleException(HttpContext context, ErrorCodeException exception)
        {
            context.Response.StatusCode = exception.ErrorCode;

            if (exception.Message != string.Empty)
            {
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDto
                {
                    Message = exception.Message
                }));
            }
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError(exception, "");
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IServiceCollection AddExceptionHandlingMiddleware(this IServiceCollection services)
        {
            return services.AddSingleton<ExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
