using LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.WebhooksManagement.DbMigrator.EntityFrameworkCore;

[ConnectionStringName("WebhooksManagementDbMigrator")]
public class WebhooksManagementMigrationsDbContext : AbpDbContext<WebhooksManagementMigrationsDbContext>
{
    public WebhooksManagementMigrationsDbContext(DbContextOptions<WebhooksManagementMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureWebhooksManagement();
    }
}
