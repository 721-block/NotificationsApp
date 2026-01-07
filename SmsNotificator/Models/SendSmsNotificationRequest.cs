namespace SmsNotificator.Models;

public class SendSmsNotificationRequest
{
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}