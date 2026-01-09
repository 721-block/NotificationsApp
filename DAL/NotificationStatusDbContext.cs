using Microsoft.EntityFrameworkCore;

namespace DAL;

public class NotificationStatusDbContext : DbContext
{
    public NotificationStatusDbContext(DbContextOptions<NotificationStatusDbContext> options) : base(options) {}
    
    public DbSet<Notification> Notifications { get; set; }
}