using Files.Application.Responses;
using Files.Domain.Aggregates.ImageAggregate;

namespace Files.Application.Queries;

public class ImageQueries : IImageQueries
{
    private readonly IImageRepository _imageRepository;

    public ImageQueries(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }
    
    public async Task<DownloadImageResponse?> DownloadAsync(string groupId, string sizeName)
    {
        var imageId = Image.CreateId(groupId, sizeName);
        var image = await _imageRepository.DownloadAsync(imageId);
        if (image is null)
            return null;

        return new DownloadImageResponse
        {
            Name = image.Id.Key,
            ContentType = image.ContentType,
            Content = image.Content
        };
    }
}