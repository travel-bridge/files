using Files.Domain.Exceptions;
using FluentValidation;

namespace Files.Domain.Aggregates.ImageAggregate;

public class ImageValidator : AbstractValidator<Image>
{
    private static readonly string[] AllowedContentTypes =
    {
        "image/bmp",
        "image/gif",
        "image/jpeg",
        "image/x-portable-bitmap",
        "image/png",
        "image/tiff",
        "image/webp"
    };

    public ImageValidator()
    {
        RuleFor(x => x.Id.BucketName)
            .NotEmpty()
            .WithState(_ => new InvalidRequestMessage("BucketName should not be empty."));
        
        RuleFor(x => x.Id.Key)
            .NotEmpty()
            .WithState(_ => new InvalidRequestMessage("BucketName should not be empty."));
        
        RuleFor(x => x.ContentType)
            .Must(x => AllowedContentTypes.Contains(x))
            .WithState(_ => new ValidationMessage(
                Array.Empty<string>(),
                "Validation:ImageContentTypeAllowedError"));
        
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithState(_ => new ValidationMessage(
                Array.Empty<string>(),
                "Validation:ImageContentNotEmptyError"));
    }
}