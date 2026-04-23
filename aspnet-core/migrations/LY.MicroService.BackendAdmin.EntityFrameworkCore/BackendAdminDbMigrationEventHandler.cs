using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore;

public class BackendAdminDbMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<BackendAdminMigrationsDbContext>
{
    protected BackendAdminDataSeeder DataSeeder { get; }

    public BackendAdminDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        BackendAdminDataSeeder dataSeeder)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<BackendAdminMigrationsDbContext>(),
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task AfterTenantCreated(TenantCreatedEto eventData, bool schemaMigrated)
    {
        // 新租户数据种子
        var context = new DataSeedContext(eventData.Id);
        if (eventData.Properties != null)
        {
            foreach (var property in eventData.Properties)
            {
                context.WithProperty(property.Key, property.Value);
            }
        }

        await DataSeeder.SeedAsync(context);
    }
}
