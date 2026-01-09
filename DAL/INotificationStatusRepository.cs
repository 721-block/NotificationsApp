namespace DAL;

public interface INotificationStatusRepository
{
    Task UpdateNotificationStatus(Guid notificationId, NotificationStatus status);
    Task<Notification?> GetById(Guid id);
}