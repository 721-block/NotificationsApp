using MainSms;
using Microsoft.Extensions.Options;

namespace SmsNotificator.Sms;

public interface ISmsMessageProvider
{
    SmsMessage Get();
}

public class SmsMessageProvider : ISmsMessageProvider
{
    private SmsMessage smsMessage;
    private readonly Lock locker = new();

    public SmsMessageProvider(IOptionsMonitor<MainSmsSettings> smsSettingsMonitor)
    {
        smsSettingsMonitor.OnChange(settings =>
            {
                lock (locker)
                {
                    smsMessage = new SmsMessage(settings.ProjectId, settings.ApiKey, settings.IsTest, settings.UseSsl);
                }
            }
        );

        var smsSettings = smsSettingsMonitor.CurrentValue;
        smsMessage = new SmsMessage(smsSettings.ProjectId, smsSettings.ApiKey, smsSettings.IsTest, smsSettings.UseSsl);
    }

    public SmsMessage Get()
    {
        lock (locker)
        {
            return smsMessage;
        }
    }
}