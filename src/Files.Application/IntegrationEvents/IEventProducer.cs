namespace Files.Application.IntegrationEvents;

public interface IEventProducer : IDisposable
{
    Task ProduceAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent;
}