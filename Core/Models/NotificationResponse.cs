namespace Core.Models;

public class NotificationResponse
{
    public NotificationRequest NotificationRequest;
    public bool IsNotificationSent;
    public string? Error;
}