using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MicroService.MessageService;
public class MessageServiceDbMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<MessageServiceMigrationsDbContext>
{
    protected MessageServiceDataSeeder MessageServiceDataSeeder { get; }

    public MessageServiceDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        MessageServiceDataSeeder messageServiceDataSeeder)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<MessageServiceMigrationsDbContext>(),
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        MessageServiceDataSeeder = messageServiceDataSeeder;
    }

    protected async override Task AfterTenantCreated(TenantCreatedEto eventData, bool schemaMigrated)
    {
        using (CurrentTenant.Change(eventData.Id))
        {
            var context = new DataSeedContext(eventData.Id);
            if (eventData.Properties != null)
            {
                foreach (var prop in eventData.Properties)
                {
                    context.WithProperty(prop.Key, prop.Value);
                }
            }
            await MessageServiceDataSeeder.SeedAsync(context);
        }
    }
}
