using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCookbook.Comunicacao.Response;
using SmartCookbook.Exceptions;
using SmartCookbook.Exceptions.ExceptionsBase;
using System.Net;

namespace SmartCookbook.API.Filters;

public class FilterExceptions : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is SmartCookbookException)
        {
            SmartCookbookExceptionHandler(context);
        }
        else
        {
            ThrowUnkownError(context);
        }
    }

    private void SmartCookbookExceptionHandler(ExceptionContext context)
    {
        if (context.Exception is ValidationErrorException)
        {
            ValidationErrorExceptionHandler(context);
        }
        else if(context.Exception is InvalidLoginException)
            InvalidLoginExceptionHandler(context);
    }

    private void ValidationErrorExceptionHandler(ExceptionContext context)
    {
        var validationErrorException = context.Exception as ValidationErrorException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ErrorResponseJson(validationErrorException!.ErrorMessages));
    }

    private void ThrowUnkownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ErrorResponseJson(ResourceErrorMessages.UNKOWN_ERROR));
    }

    private void InvalidLoginExceptionHandler(ExceptionContext context)
    {
        var loginError = context.Exception as InvalidLoginException;
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new ErrorResponseJson(loginError.Message));
    }
}
