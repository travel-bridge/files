using System.Globalization;
using System.Reflection;
using Files.Domain.Aggregates.ImageAggregate;
using Microsoft.Extensions.Options;

namespace Files.Infrastructure.Files;

public class ImageRepository : FileRepositoryBase, IImageRepository
{
    public ImageRepository(IOptions<FilesOptions> filesOptions) : base(filesOptions)
    {
    }

    public async Task UploadAsync(Image entity, CancellationToken cancellationToken = default)
        => await UploadAsync(
            entity.Id.BucketName,
            entity.Id.Key,
            entity.ContentType,
            entity.Content,
            cancellationToken);

    public async Task<Image?> DownloadAsync(ImageId id, CancellationToken cancellationToken = default)
    {
        var response = await DownloadAsync(id.BucketName, id.Key, cancellationToken);
        if (!response.HasValue)
            return null;

        var contentType = response.Value.ContentType;
        var content = response.Value.Content;

        var image = (Image)Activator.CreateInstance(
            typeof(Image), 
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, 
            null, 
            new object[] { id.BucketName, id.Key, contentType, content }, 
            CultureInfo.InvariantCulture)!;
        
        return image;
    }

    public async Task DeleteAsync(ImageId id, CancellationToken cancellationToken = default)
        => await DeleteAsync(id.BucketName, id.Key, cancellationToken);
}