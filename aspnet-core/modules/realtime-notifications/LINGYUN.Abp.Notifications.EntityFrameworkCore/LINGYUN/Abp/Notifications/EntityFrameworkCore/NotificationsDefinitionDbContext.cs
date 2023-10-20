using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpNotificationsDbProperties.ConnectionStringName)]
public class NotificationsDefinitionDbContext : AbpDbContext<NotificationsDefinitionDbContext>, INotificationsDefinitionDbContext
{
    public NotificationsDefinitionDbContext(DbContextOptions<NotificationsDefinitionDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetMultiTenancySide(MultiTenancySides.Host);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureNotificationsDefinition();
    }
}
