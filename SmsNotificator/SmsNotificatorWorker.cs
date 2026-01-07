using RabbitMqModule.RpcServer;

namespace SmsNotificator;

public class SmsNotificatorWorker(
    IRpcServer rpcServer,
    ILogger<SmsNotificatorWorker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        await rpcServer.Start().ConfigureAwait(false);
    }
}