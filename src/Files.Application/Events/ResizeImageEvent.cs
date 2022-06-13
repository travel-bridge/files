namespace Files.Application.Events;

public record ResizeImageEvent(string GroupId) : IEvent
{
    public string GetTopic() => Topics.ResizeImage;
}