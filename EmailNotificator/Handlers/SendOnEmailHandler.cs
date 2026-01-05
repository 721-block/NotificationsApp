using EmailNotificator.Models;
using RabbitMqModule.RpcServer;

namespace EmailNotificator.Handlers;

public class SendOnEmailHandler(IMailService mailService) : IRpcServerHandler<SendNotificationRequest, SendNotificationResponse>
{
    public async Task<ResponseData<SendNotificationResponse>> Handle(
        SendNotificationRequest requestMessage,
        ResponseData<SendNotificationResponse> responseData
    )
    {
        var mailData = new MailData
        {
            RecipientAddresses = [requestMessage.Recipient],
            Subject = requestMessage.Subject,
            Body = requestMessage.Body
        };

        var result = await mailService.Send(mailData).ConfigureAwait(false);

        return new ResponseData<SendNotificationResponse>
        {
            Message = new SendNotificationResponse {IsNotificationSent = result }
        };
    }
}