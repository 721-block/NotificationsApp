namespace SmsNotificator.Sms;

public class MainSmsSettings
{
    public string ProjectId { get; set; }
    public string ApiKey { get; set; }
    public bool IsTest { get; set; }
    public bool UseSsl { get; set; }
}