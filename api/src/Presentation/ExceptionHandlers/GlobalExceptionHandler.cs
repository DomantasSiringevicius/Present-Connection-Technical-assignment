using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Presentation.ExceptionHandlers;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail, code) = exception switch
        {
            BookingRuleException bookingRuleException => (StatusCodes.Status400BadRequest, "Booking rule violation",
                bookingRuleException.Message, bookingRuleException.Code),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found", exception.Message,
                "RESOURCE_NOT_FOUND"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred",
                "Something went wrong. Please try again later.", "INTERNAL_SERVER_ERROR")
        };
        
        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);
        }

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Extensions = { ["code"] = code }
            }
        });
    }
}