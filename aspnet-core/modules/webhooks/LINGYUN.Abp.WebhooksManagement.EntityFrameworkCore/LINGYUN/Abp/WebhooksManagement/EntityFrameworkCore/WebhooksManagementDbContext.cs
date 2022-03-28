using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

[ConnectionStringName(WebhooksManagementDbProperties.ConnectionStringName)]
public class WebhooksManagementDbContext : AbpDbContext<WebhooksManagementDbContext>, IWebhooksManagementDbContext
{
    public WebhooksManagementDbContext(
        DbContextOptions<WebhooksManagementDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureWebhooksManagement();
    }
}
