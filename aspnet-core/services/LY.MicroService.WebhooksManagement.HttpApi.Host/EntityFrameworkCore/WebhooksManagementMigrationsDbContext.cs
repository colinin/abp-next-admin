using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LY..WebhooksManagement.EntityFrameworkCore;

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
