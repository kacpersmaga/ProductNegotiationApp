using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.API.Common;

namespace Shared.Infrastructure.Middleware;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var statusCode = StatusCodes.Status500InternalServerError;
            string message = "An unexpected error occurred";
            
            switch (exception)
            {
                case ApplicationException appEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = appEx.Message;
                    break;
                
                case InvalidOperationException opEx when opEx.Message.Contains("not found", StringComparison.OrdinalIgnoreCase):
                    statusCode = StatusCodes.Status404NotFound;
                    message = opEx.Message;
                    break;
                    
                case InvalidOperationException opEx when opEx.Message.Contains("Maximum number of proposals", StringComparison.OrdinalIgnoreCase):
                    statusCode = StatusCodes.Status400BadRequest;
                    message = opEx.Message;
                    break;
                    
                case ArgumentException argEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = argEx.Message;
                    break;
                
                
                default:
                    message = "An unexpected error occurred. Please try again later.";
                    break;
            }
            
            context.Response.StatusCode = statusCode;
            
            var response = ApiResponse<string>.Fail(message);
            
            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }