using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

public class ProjectNameDbMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<ProjectNameDbContext>
{
    protected ProjectNameDataSeeder DataSeeder { get; }

    public ProjectNameDbMigrationEventHandler(
        ProjectNameDataSeeder dataSeeder,
        ITenantStore tenantStore,
        ICurrentTenant currentTenant, 
        IUnitOfWorkManager unitOfWorkManager, 
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus, 
        ILoggerFactory loggerFactory) 
        : base(
            ConnectionStringNameAttribute.GetConnStringName<ProjectNameDbContext>(),
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
