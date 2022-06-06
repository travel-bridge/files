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
    
    private static readonly string[] Location = { "image" };

    public ImageValidator()
    {
        RuleFor(x => x.Id.BucketName)
            .NotEmpty()
            .WithState(_ => new InvalidRequestMessage("BucketName should not be empty."));
        
        RuleFor(x => x.Id.Key)
            .NotEmpty()
            .WithState(_ => new InvalidRequestMessage("BucketName should not be empty."));

        var contentTypeLocation = new List<string>(Location) { "contentType" };
        RuleFor(x => x.ContentType)
            .Must(x => AllowedContentTypes.Contains(x))
            .WithState(_ => new ValidationMessage(
                contentTypeLocation,
                "Validation:ImageContentTypeAllowedError"));
        
        var contentLocation = new List<string>(Location) { "content" };
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithState(_ => new ValidationMessage(
                contentLocation,
                "Validation:ImageContentNotEmptyError"));
    }
}