namespace Files.Application.IntegrationEvents;

public record ResizeImageIntegrationEvent(string BucketName, string Name) : IIntegrationEvent
{
    public string GetTopic() => Topics.ResizeImage;
}