using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpNotificationsDbProperties.ConnectionStringName)]
public interface INotificationsDefinitionDbContext : IEfCoreDbContext
{
}
