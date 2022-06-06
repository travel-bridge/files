namespace Files.Application.IntegrationEvents;

public interface IEventConsumer : IDisposable
{
    Task ConsumeAndHandleAsync<TEvent>(
        Func<TEvent, Task> handle,
        CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent;
}