using RabbitMqModule.RpcServer;

namespace PushNotificator;

public class PushNotificatorWorker(
    IRpcServer rpcServer,
    ILogger<PushNotificatorWorker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        await rpcServer.Start().ConfigureAwait(false);
    }
}