using RabbitMqModule.RpcServer;
using SmsNotificator.Models;
using SmsNotificator.Sms;

namespace SmsNotificator.Handlers;

public class SendSmsHandler(IServiceScopeFactory serviceScopeFactory) : IRpcServerHandler<SendSmsNotificationRequest, SendSmsNotificationResponse>
{
    public async Task<SendSmsNotificationResponse> Handle(SendSmsNotificationRequest requestMessage)
    {
        await using var serviceScope = serviceScopeFactory.CreateAsyncScope();
        var smsService = serviceScope.ServiceProvider.GetRequiredService<ISmsService>();
        var smsSettings = serviceScope.ServiceProvider.GetRequiredService<MainSmsSettings>();
        var smsData = new SmsData
        {
            SenderName = smsSettings.SenderName,
            Body = requestMessage.Body,
            Recipients = [requestMessage.Recipient],
            Subject = requestMessage.Subject
        };

        var result = await smsService.Send(smsData).ConfigureAwait(false);

        return new SendSmsNotificationResponse
        {
            IsNotificationSent = result.IsSent,
            Error = result.Message
        };
    }
}