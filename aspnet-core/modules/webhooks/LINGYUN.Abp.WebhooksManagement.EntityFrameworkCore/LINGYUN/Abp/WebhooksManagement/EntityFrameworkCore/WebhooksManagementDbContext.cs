using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(WebhooksManagementDbProperties.ConnectionStringName)]
public class WebhooksManagementDbContext : AbpDbContext<WebhooksManagementDbContext>, IWebhooksManagementDbContext
{
    public DbSet<WebhookSendRecord> WebhookSendRecord { get; set; }
    public DbSet<WebhookGroupDefinitionRecord> WebhookGroupDefinitionRecords { get; set; }
    public DbSet<WebhookDefinitionRecord> WebhookDefinitionRecords { get; set; }
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
