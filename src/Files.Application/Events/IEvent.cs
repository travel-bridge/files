namespace Files.Application.Events;

public interface IEvent
{
    string GetTopic();
}