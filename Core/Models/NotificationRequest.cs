namespace Core.Models;

public class NotificationRequest
{
    public Guid Id { get; set; }
    public short RetryCount { get; set; } = 0;
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}