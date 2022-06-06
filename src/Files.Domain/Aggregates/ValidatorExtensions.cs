using Files.Domain.Exceptions;
using FluentValidation;

namespace Files.Domain.Aggregates;

internal static class ValidatorExtensions
{
    public static void ValidateEntityAndThrow<TEntity>(this IValidator<TEntity> validator, TEntity entity)
    {
        var result = validator.Validate(entity);
        if (result.IsValid)
            return;

        var firstInvalidRequestMessage = result.Errors
            .Where(x => x.CustomState is InvalidRequestMessage)
            .Select(x => (InvalidRequestMessage) x.CustomState)
            .FirstOrDefault();

        if (firstInvalidRequestMessage is not null)
            throw new InvalidRequestException(firstInvalidRequestMessage);

        var validationMessages = result.Errors
            .Where(x => x.CustomState is ValidationMessage)
            .Select(x => (ValidationMessage) x.CustomState)
            .ToList();

        throw new Exceptions.ValidationException(validationMessages);
    }
}