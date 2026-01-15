using LINGYUN.Abp.WebhooksManagement;
using LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.WebhookService;

[ConnectionStringName("Default")]
public class WebhookServiceMigrationsDbContext : 
    AbpDbContext<WebhookServiceMigrationsDbContext>,
    IWebhooksManagementDbContext
{
    public DbSet<WebhookSendRecord> WebhookSendRecord { get; set; }

    public DbSet<WebhookGroupDefinitionRecord> WebhookGroupDefinitionRecords { get; set; }

    public DbSet<WebhookDefinitionRecord> WebhookDefinitionRecords { get; set; }

    public WebhookServiceMigrationsDbContext(DbContextOptions<WebhookServiceMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureWebhooksManagement();
    }
}
