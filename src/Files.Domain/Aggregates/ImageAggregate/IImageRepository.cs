namespace Files.Domain.Aggregates.ImageAggregate;

public interface IImageRepository
{
    Task UploadAsync(Image image, CancellationToken cancellationToken = default);

    Task<Image?> DownloadAsync(ImageId id, CancellationToken cancellationToken = default);

    Task DeleteAsync(ImageId id, CancellationToken cancellationToken = default);
}