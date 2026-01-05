using EmailNotificator.Models;
using RabbitMqModule.RpcServer;

namespace EmailNotificator.Handlers;

public class SendOnEmailHandler(IMailService mailService) : IRpcServerHandler<SendNotificationRequest, SendNotificationResponse>
{
    public async Task<SendNotificationResponse> Handle(SendNotificationRequest requestMessage)
    {
        var mailData = new MailData
        {
            RecipientAddresses = [requestMessage.Recipient],
            Subject = requestMessage.Subject,
            Body = requestMessage.Body
        };

        var result = await mailService.Send(mailData).ConfigureAwait(false);

        return new SendNotificationResponse { IsNotificationSent = result };
    }
}