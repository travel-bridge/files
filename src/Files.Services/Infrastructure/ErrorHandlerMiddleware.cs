using Files.Application.Resources;
using Files.Domain.Exceptions;
using Microsoft.Extensions.Localization;

namespace Files.Services.Infrastructure;

public class ErrorHandlerMiddleware
{
    private const string DefaultCategory = "Default";

    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IStringLocalizer<FileResource> _stringLocalizer;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger,
        IStringLocalizer<FileResource> stringLocalizer)
    {
        _next = next;
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationException)
        {
            context.Response.StatusCode = validationException.StatusCode;
            var response = GetValidationResponse(validationException, _stringLocalizer);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (DomainException domainException)
        {
            context.Response.StatusCode = domainException.StatusCode;
            var response = GetBaseResponse(domainException, _stringLocalizer);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ExceptionBase exceptionBase)
        {
            _logger.LogError(exceptionBase, exceptionBase.Message);
            context.Response.StatusCode = exceptionBase.StatusCode;
            var response = GetBaseResponse(exceptionBase, _stringLocalizer);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new ErrorResponseCollection(
                DefaultCategory,
                new[] { new ErrorResponse(exception.Message, Array.Empty<string>()) });
            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static ErrorResponseCollection GetValidationResponse(
        ValidationException validationException,
        IStringLocalizer stringLocalizer)
    {
        var errors = validationException.Messages
            .Select(message => new ErrorResponse(
                stringLocalizer.GetString(message.Message, message.MessageParameters),
                message.Location))
            .ToArray();

        return new ErrorResponseCollection(validationException.Category, errors);
    }

    private static ErrorResponseCollection GetBaseResponse(
        ExceptionBase exceptionBase,
        IStringLocalizer stringLocalizer)
    {
        var localizedMessage = exceptionBase.IsLocalized
            ? stringLocalizer.GetString(exceptionBase.Message, exceptionBase.MessageParameters)
            : exceptionBase.Message;

        var errors = new[] { new ErrorResponse(localizedMessage, Array.Empty<string>()) };

        return new ErrorResponseCollection(exceptionBase.Category, errors);
    }

    private record ErrorResponseCollection(string Code, IReadOnlyCollection<ErrorResponse> Errors);

    private record ErrorResponse(string Message, IReadOnlyCollection<string> Location);
}