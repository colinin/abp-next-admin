using LINGYUN.Abp.Saas.Tenants;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.TaskManagement.EntityFrameworkCore;

public class TaskManagementDbMigrationEventHandler : 
    EfCoreDatabaseMigrationEventHandlerBase<TaskManagementMigrationsDbContext>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>
{
    protected TaskManagementDataSeeder DataSeeder { get; }

    public TaskManagementDbMigrationEventHandler(
       ICurrentTenant currentTenant,
       IUnitOfWorkManager unitOfWorkManager,
       ITenantStore tenantStore,
       IAbpDistributedLock abpDistributedLock,
       IDistributedEventBus distributedEventBus,
       ILoggerFactory loggerFactory,
       TaskManagementDataSeeder dataSeeder)
       : base("TaskManagementDbMigrator", currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
    }

    public async Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
    {
        // 租户删除时移除轮询作业
        await DataSeeder.RemoveSeedAsync(eventData.Entity.Id);
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
