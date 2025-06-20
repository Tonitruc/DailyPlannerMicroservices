using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseBuldingsBlocks.Middleware;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, 
        CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException => HandleValidationException(validationException),
            _ => HandleException(exception)
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }

    private ProblemDetails HandleValidationException(ValidationException exception)
    {
        var problemDetails = new ValidationProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = exception.GetType().Name,
            Detail = exception.Message
        };

        problemDetails.Errors.Add(exception.GetType().Name, [.. exception.Errors.Select(error => error.ErrorMessage)]);

        return problemDetails;
    }

    private ProblemDetails HandleException(Exception exception)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = exception.GetType().Name,
            Detail = exception.Message
        };
    }
}