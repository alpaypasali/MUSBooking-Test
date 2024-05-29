using Infrastructure.Exceptions.Types;
using Infrastructure.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions.Handlers;

    public abstract class ExceptionHandler
    {
        public Task HandleExceptionAsync(Exception exception) =>
            exception switch
            {

                BusinessException businessException => HandleException(businessException),
                ValidationException validationException => HandleException(validationException),
                _ => HandleExceptionAsync(exception)
            };
        protected abstract Task HandleException(BusinessException businessException);
        protected abstract Task HandleException(ValidationException validationException);

        protected abstract Task HandleException(Exception exception);


    }
