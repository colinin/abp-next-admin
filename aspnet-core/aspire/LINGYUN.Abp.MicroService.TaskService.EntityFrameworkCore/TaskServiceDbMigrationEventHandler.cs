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

namespace LINGYUN.Abp.MicroService.TaskService;
public class TaskServiceDbMigrationEventHandler :
    EfCoreDatabaseMigrationEventHandlerBase<TaskServiceMigrationsDbContext>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>
{
    protected TaskServiceDataSeeder DataSeeder { get; }

    public TaskServiceDbMigrationEventHandler(
       ICurrentTenant currentTenant,
       IUnitOfWorkManager unitOfWorkManager,
       ITenantStore tenantStore,
       IAbpDistributedLock abpDistributedLock,
       IDistributedEventBus distributedEventBus,
       ILoggerFactory loggerFactory,
       TaskServiceDataSeeder dataSeeder)
       : base(
            ConnectionStringNameAttribute.GetConnStringName<TaskServiceMigrationsDbContext>(), 
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
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

