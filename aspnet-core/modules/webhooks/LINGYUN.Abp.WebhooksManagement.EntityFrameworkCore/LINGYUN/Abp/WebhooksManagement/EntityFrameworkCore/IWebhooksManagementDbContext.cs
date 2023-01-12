using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(WebhooksManagementDbProperties.ConnectionStringName)]
public interface IWebhooksManagementDbContext : IEfCoreDbContext
{
}
