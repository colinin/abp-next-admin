using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

[ConnectionStringName(AbpNotificationsDbProperties.ConnectionStringName)]
public class NotificationsDbContext : AbpDbContext<NotificationsDbContext>, INotificationsDbContext
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<UserSubscribe> UserSubscribes { get; set; }

    public DbSet<NotificationDefinitionGroupRecord> NotificationDefinitionGroupRecords { get; set; }
    public DbSet<NotificationDefinitionRecord> NotificationDefinitionRecords { get; set; }

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureNotifications();
    }
}
