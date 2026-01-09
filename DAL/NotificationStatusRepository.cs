namespace DAL;

public class NotificationStatusRepository(NotificationStatusDbContext context) : INotificationStatusRepository
{
    public async Task UpdateNotificationStatus(Guid notificationId, NotificationStatus status)
    {
        var notification = new Notification { Id = notificationId, NotificationStatus = status };
        await context.Notifications.AddAsync(notification);
    }

    public async Task<Notification?> GetById(Guid id)
    {
        return await context.Notifications.FindAsync(id);
    }
}