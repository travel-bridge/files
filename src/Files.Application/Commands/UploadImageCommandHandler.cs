using Files.Application.Events;
using Files.Application.Responses;
using Files.Domain.Aggregates.ImageAggregate;
using MediatR;

namespace Files.Application.Commands;

public record UploadImageCommand(string Name, string ContentType, byte[] Content) : IRequest<UploadImageResponse>;

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, UploadImageResponse>
{
    private readonly IImageRepository _imageRepository;
    private readonly IEventProducer _eventProducer;

    public UploadImageCommandHandler(IImageRepository imageRepository, IEventProducer eventProducer)
    {
        _imageRepository = imageRepository;
        _eventProducer = eventProducer;
    }
    
    public async Task<UploadImageResponse> Handle(UploadImageCommand command, CancellationToken cancellationToken)
    {
        var originalImage = Image.CreateOriginal(command.Name, command.ContentType, command.Content);
        await _imageRepository.UploadAsync(originalImage, cancellationToken);
        await _eventProducer.ProduceAsync(
            new ResizeImageEvent(originalImage.GetGroupId()),
            cancellationToken);
        
        return new UploadImageResponse { GroupId = originalImage.GetGroupId() };
    }
}