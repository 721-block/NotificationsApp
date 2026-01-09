using Core.Models;
using RabbitMqModule.RpcClient;

namespace ApiGateway.Services;

public class NotificationProviderWorker<TNotificationRequest, TNotificationResponse>(
    IRpcClient<TNotificationRequest, TNotificationResponse> rpcClient,
    ILogger<NotificationProviderWorker<TNotificationRequest, TNotificationResponse>> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        await rpcClient.Start().ConfigureAwait(false);
    }
}