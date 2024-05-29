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
using Infrastructure.Exceptions.Handlers;

namespace Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpExceptionHandler _httpExceptionHandler;
        private readonly IHttpContextAccessor _contextAccessor;
 

        public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor contextAccessor)
        {
            _next = next;
            _httpExceptionHandler = new HttpExceptionHandler();
            _contextAccessor = contextAccessor;
       
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
               
                await HandleExceptionAsync(context.Response, exception);
            }

        }

      

        private Task HandleExceptionAsync(HttpResponse response, Exception exception)
        {
            response.ContentType = "application/json";
            _httpExceptionHandler.response = response;
            return _httpExceptionHandler.HandleExceptionAsync(exception);
        }
    }
}
