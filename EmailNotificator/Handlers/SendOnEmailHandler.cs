using EmailNotificator.Models;
using RabbitMqModule.RpcServer;

namespace EmailNotificator.Handlers;

public class SendOnEmailHandler(IServiceScopeFactory serviceScopeFactory) : IRpcServerHandler<SendMailNotificationRequest, SendMailNotificationResponse>
{
    public async Task<SendMailNotificationResponse> Handle(SendMailNotificationRequest requestMessage)
    {
        await using var serviceScope = serviceScopeFactory.CreateAsyncScope();
        var mailService = serviceScope.ServiceProvider.GetRequiredService<IMailService>();

        var mailData = new MailData
        {
            RecipientAddresses = [requestMessage.Recipient],
            Subject = requestMessage.Subject,
            Body = requestMessage.Body
        };

        var result = await mailService.Send(mailData).ConfigureAwait(false);

        return new SendMailNotificationResponse { IsNotificationSent = result.IsSent, Error = result.Message };
    }
}