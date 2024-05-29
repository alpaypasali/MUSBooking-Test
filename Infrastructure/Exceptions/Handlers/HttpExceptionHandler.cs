using Infrastructure.Exceptions.Extensions;
using Infrastructure.Exceptions.HttpProblemDetails;
using Infrastructure.Exceptions.Types;
using Infrastructure.NewFolder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions.Handlers;
public class HttpExceptionHandler : ExceptionHandler
{
    private HttpResponse _response;
    public HttpResponse response
    {


        get => _response ?? throw new ArgumentNullException(nameof(_response));
        set => _response = value;
    }
    protected override Task HandleException(BusinessException businessException)
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        string details = new BusinessProblemDetails(businessException.Message).AsJson();
        return response.WriteAsync(details);
    }

    protected override Task HandleException(Exception exception)
    {
        response.StatusCode = StatusCodes.Status500InternalServerError;
        string details = new InternalServerProblemDetails(exception.Message).AsJson();
        return response.WriteAsync(details);
    }

    protected override Task HandleException(ValidationException validationException)
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        string details = new ValidationProblemDetails(validationException.Errors).AsJson();
        return response.WriteAsync(details);
    }
}

