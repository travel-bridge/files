namespace Files.Application.Responses;

public class DownloadImageResponse
{
    public string Name { get; set; } = null!;
    
    public string ContentType { get; set; } = null!;

    public byte[] Content { get; set; } = null!;
}