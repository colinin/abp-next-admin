using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.WebhooksManagement.EntityFrameworkCore;

public class WebhooksManagementDbMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<WebhooksManagementMigrationsDbContext>
{
    protected IDataSeeder DataSeeder { get; }

    public WebhooksManagementDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        IDataSeeder dataSeeder)
        : base("WebhooksManagementDbMigrator", currentTenant, unitOfWorkManager, tenantStore, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task AfterTenantCreated(TenantCreatedEto eventData, bool schemaMigrated)
    {
        if (!schemaMigrated)
        {
            return;
        }
        using (CurrentTenant.Change(eventData.Id))
        {
            await DataSeeder.SeedAsync(eventData.Id);
        }
    }
}
