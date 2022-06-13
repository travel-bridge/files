using Files.Application.Events;
using Files.Domain;
using Files.Domain.Aggregates;
using Files.Domain.Aggregates.ImageAggregate;
using Files.Infrastructure.Events;
using Files.Infrastructure.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Files.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddFiles(configuration)
            .AddEvents(configuration);

    private static IServiceCollection AddFiles(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<FilesOptions>()
            .Bind(configuration.GetSection(FilesOptions.SectionKey))
            .ValidateDataAnnotations();

        services.AddSingleton<IImageRepository, ImageRepository>();
        
        return services;
    }

    private static IServiceCollection AddEvents(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<EventsOptions>()
            .Bind(configuration.GetSection(EventsOptions.SectionKey))
            .ValidateDataAnnotations();

        services.AddSingleton<IEventProducer, EventProducer>();
        services.AddSingleton<IEventConsumerFactory, EventConsumerFactory>();
        
        return services;
    }
}