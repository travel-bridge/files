namespace Files.Application.Events;

public interface IEventProducer : IDisposable
{
    Task ProduceAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}