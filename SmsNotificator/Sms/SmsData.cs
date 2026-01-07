namespace SmsNotificator.Sms;

public class SmsData
{
    public string SenderName { get; set; }
    public string[] Recipients { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}