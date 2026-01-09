namespace DAL;

public class Notification
{
    public Guid Id { get; set; }
    public NotificationStatus NotificationStatus { get; set; }
}

public enum NotificationStatus
{
    Unknown = 0,
    Success,
    Pending,
    Failed,
    Retry
}