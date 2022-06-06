using Confluent.Kafka;
using Files.Application.IntegrationEvents;
using Microsoft.Extensions.Options;

namespace Files.Infrastructure.Events;

public class EventConsumerFactory : IEventConsumerFactory
{
    private readonly EventsOptions _eventsOptions;

    public EventConsumerFactory(IOptions<EventsOptions> eventsOptions)
    {
        _eventsOptions = eventsOptions.Value;
    }
    
    public IEventConsumer Subscribe(string topic, string groupId)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _eventsOptions.BootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        
        var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);
        var eventConsumer = new EventConsumer(consumer);
        
        return eventConsumer;
    }
}