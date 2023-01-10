using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

[ConnectionStringName(AbpNotificationsDbProperties.ConnectionStringName)]
public interface INotificationsDbContext : IEfCoreDbContext
{
}
