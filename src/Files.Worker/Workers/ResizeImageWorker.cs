using Files.Application.Commands;
using Files.Application.Events;
using Files.Application.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Files.Worker.Workers;

public class ResizeImageWorker : WorkerBase
{
    private const string ConsumerGroupId = "resize-image-consumer-group";
    
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IEventConsumerFactory _eventConsumerFactory;

    public ResizeImageWorker(
        IServiceScopeFactory serviceScopeFactory,
        IEventConsumerFactory eventConsumerFactory,
        IStringLocalizer<FileResource> stringLocalizer,
        ILogger<WorkerBase> logger)
        : base(stringLocalizer, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _eventConsumerFactory = eventConsumerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        using var eventConsumer = _eventConsumerFactory.Subscribe(
            Topics.ResizeImage,
            ConsumerGroupId);
        
        do
        {
            await ExecuteSafelyAsync(
                async () =>
                {
                    await eventConsumer.ConsumeAndHandleAsync<ResizeImageEvent>(
                        async @event => await mediator.Send(
                            new ResizeImageCommand(@event.GroupId),
                            stoppingToken),
                        stoppingToken);
                },
                stoppingToken);
        }
        while (!stoppingToken.IsCancellationRequested);
    }
}