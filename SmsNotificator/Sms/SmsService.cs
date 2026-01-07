using MainSms;

namespace SmsNotificator.Sms;

public interface ISmsService
{
    Task<SendingResult> Send(SmsData smsData);
}

public class SmsService(ISmsMessageProvider smsMessageProvider) : ISmsService
{
    public Task<SendingResult> Send(SmsData smsData)
    {
        var smsMessage = smsMessageProvider.Get();

        var recipients = string.Join(',', smsData.Recipients);
        var content = $"{smsData.Subject}. {smsData.Body}";
        var responseSend = smsMessage.sendSms(smsData.SenderName, recipients, content);


        SendingResult result;
        if (responseSend.status == "success")
        {
            result = new SendingResult { IsSent = true };
        }
        else
        {
            result = new SendingResult
            {
                IsSent = false,
                Message = responseSend.message
            };
        }

        return Task.FromResult(result);
    }
}

public class SendingResult
{
    public bool IsSent { get; set; }
    public string? Message { get; set; }
}