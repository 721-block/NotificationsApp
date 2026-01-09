using System.Text.Json.Serialization;

namespace ApiGateway.Models;

[JsonDerivedType(typeof(PushNotification), nameof(PushNotification))]
[JsonDerivedType(typeof(EmailNotification), nameof(EmailNotification))]
[JsonDerivedType(typeof(SmsNotification), nameof(SmsNotification))]
public abstract class NotificationBase
{
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}