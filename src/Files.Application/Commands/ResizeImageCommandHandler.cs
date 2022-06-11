using Files.Application.Responses;
using Files.Domain.Aggregates.ImageAggregate;
using MediatR;

namespace Files.Application.Commands;

public record ResizeImageCommand(string GroupId) : IRequest<OperationResponse>;

public class ResizeImageCommandHandler : IRequestHandler<ResizeImageCommand, OperationResponse>
{
    private readonly IImageRepository _imageRepository;

    public ResizeImageCommandHandler(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }
    
    public async Task<OperationResponse> Handle(ResizeImageCommand command, CancellationToken cancellationToken)
    {
        var originalImageId = Image.CreateOriginalId(command.GroupId);
        var originalImage = await _imageRepository.DownloadAsync(originalImageId, cancellationToken);
        if (originalImage == null)
            return OperationResponse.NotSuccess;

        var images = new List<Image>
        {
            originalImage.CreateLarge(),
            originalImage.CreateMedium(),
            originalImage.CreateSmall(),
            originalImage.CreateExtraSmall(),
            originalImage.CreateMini()
        };
        
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = images.Count };
        await Parallel.ForEachAsync(images, parallelOptions, async (image, token)
            => await _imageRepository.UploadAsync(image, token));

        await _imageRepository.DeleteAsync(originalImage.Id, cancellationToken);

        return OperationResponse.Success;
    }
}