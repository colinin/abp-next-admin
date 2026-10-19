using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.MessageService.EntityFrameworkCore;

[ConnectionStringName("RealtimeMessageDbMigrator")]
public class MessageServiceMigrationsDbContext : AbpDbContext<MessageServiceMigrationsDbContext>
{
    public MessageServiceMigrationsDbContext(DbContextOptions<MessageServiceMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureNotifications();
        modelBuilder.ConfigureNotificationsDefinition();
        modelBuilder.ConfigureMessageService();
    }
}
