using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

[ConnectionStringName(WebhooksManagementDbProperties.ConnectionStringName)]
public interface IWebhooksManagementDbContext : IEfCoreDbContext
{
}
