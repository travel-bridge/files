namespace Files.Application.IntegrationEvents;

public interface IIntegrationEvent
{
    string GetTopic();
}