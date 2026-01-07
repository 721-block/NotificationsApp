using PushNotificator.Models;
using PushNotificator.Push;
using RabbitMqModule.RpcServer;

namespace PushNotificator.Handlers;

public class SendPushHandler(IServiceScopeFactory serviceScopeFactory) : IRpcServerHandler<SendPushNotificationRequest, SendPushNotificationResponse>
{
    public async Task<SendPushNotificationResponse> Handle(SendPushNotificationRequest requestMessage)
    {
        await using var serviceScope = serviceScopeFactory.CreateAsyncScope();
        var pushService = serviceScope.ServiceProvider.GetRequiredService<IPushService>();

        var pushData = new PushData
        {
            RecipientEmail = requestMessage.Recipient,
            Subject = requestMessage.Subject,
            Body = requestMessage.Body
        };
        var result = await pushService.Send(pushData).ConfigureAwait(false);

        return new SendPushNotificationResponse
        {
            IsNotificationSent = result.IsSent,
            Error = result.Message,
        };
    }
}