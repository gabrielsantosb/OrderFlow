
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
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
        catch (Exception exception) when (!context.Response.HasStarted)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        if (statusCode >= 500)
        {
            _logger.LogError(exception, "An unexpected error occurred while processing the request.");
        }
        else
        {
            _logger.LogWarning(exception, "The request was rejected.");
        }

        ProblemDetails problemDetails = new()
        {
            Status = statusCode,
            Title = statusCode == 400 ? "Invalid request" : "Internal server error",
            Detail = statusCode == 400
                ? "The request contains invalid data."
                : "An unexpected error occurred.",
            Instance = context.Request.Path
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(validationFailure => validationFailure.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(validationFailure => validationFailure.ErrorMessage).ToArray());
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken: context.RequestAborted);
    }
}
