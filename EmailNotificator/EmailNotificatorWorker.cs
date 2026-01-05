using RabbitMqModule.RpcServer;

namespace EmailNotificator;

public class EmailNotificatorWorker(
    IRpcServer rpcServer,
    ILogger<EmailNotificatorWorker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        await rpcServer.Start().ConfigureAwait(false);
    }
}
