using Files.Application.Resources;
using Files.Domain.Exceptions;
using Microsoft.Extensions.Localization;

namespace Files.Worker.Workers;

public abstract class WorkerBase : BackgroundService
{
    protected WorkerBase(
        IStringLocalizer<FileResource> stringLocalizer,
        ILogger<WorkerBase> logger)
    {
        StringLocalizer = stringLocalizer;
        Logger = logger;
    }

    protected IStringLocalizer<FileResource> StringLocalizer { get; }

    protected ILogger<WorkerBase> Logger { get; }
    
    protected async Task ExecuteSafelyAsync(Func<Task> func, CancellationToken stoppingToken)
    {
        try
        {
            await func();
        }
        catch (TaskCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        catch (ExceptionBase ex)
        {
            var localizedMessage = ex.IsLocalized
                ? StringLocalizer.GetString(ex.Message, ex.MessageParameters)
                : ex.Message;
            
            Logger.LogError(ex, localizedMessage);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
        }
    }
}