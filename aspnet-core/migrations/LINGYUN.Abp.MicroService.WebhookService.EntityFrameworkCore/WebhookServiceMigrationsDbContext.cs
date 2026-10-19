using LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.WebhookService.EntityFrameworkCore;

[ConnectionStringName("WebhooksManagementDbMigrator")]
public class WebhookServiceMigrationsDbContext : AbpDbContext<WebhookServiceMigrationsDbContext>
{
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
