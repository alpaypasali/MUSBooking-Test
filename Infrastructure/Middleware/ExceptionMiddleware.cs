using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Infrastructure.NewFolder;

namespace Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;
            var exceptionType = exception.GetType();

            if (exceptionType == typeof(BusinessException))
            {
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exceptionType == typeof(NewFolder.ValidationException))
            {
                status = HttpStatusCode.UnprocessableEntity;
                message = exception.Message;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = "Internal Server Error from the custom middleware.";
            }

            _logger.LogError($"Something went wrong: {exception}");

            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }
}
