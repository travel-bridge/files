namespace Files.Application.IntegrationEvents;

public record ResizeImageIntegrationEvent(string GroupId) : IIntegrationEvent
{
    public string GetTopic() => Topics.ResizeImage;
}