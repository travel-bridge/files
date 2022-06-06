using System.Text.Json;
using Confluent.Kafka;
using Files.Application.IntegrationEvents;

namespace Files.Infrastructure.Events;

public class EventConsumer : IEventConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;

    public EventConsumer(IConsumer<Ignore, string> consumer)
    {
        _consumer = consumer;
    }

    public async Task ConsumeAndHandleAsync<TEvent>(
        Func<TEvent, Task> handle,
        CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        var consumeResult = _consumer.Consume(cancellationToken);
        var @event = JsonSerializer.Deserialize<TEvent>(consumeResult.Message.Value)
            ?? throw new InvalidOperationException(
                $"Failed to deserialize {consumeResult.Message.Value} to {typeof(TEvent)} type.");

        await handle(@event);

        _consumer.Commit(consumeResult);
    }

    public void Dispose()
    {
        _consumer.Dispose();
        GC.SuppressFinalize(this);
    }
}