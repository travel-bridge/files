using Files.Application.Responses;

namespace Files.Application.Queries;

public interface IImageQueries
{
    Task<DownloadImageResponse?> DownloadAsync(string groupId, string sizeName);
}