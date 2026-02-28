using LINGYUN.Abp.Saas.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MicroService.PlatformService;
public class PlatformServiceDbMigrationEventHandler :
    EfCoreDatabaseMigrationEventHandlerBase<PlatformServiceMigrationsDbContext>,
    IDistributedEventHandler<TenantDeletedEto>
{
    protected PlatformServiceDataSeeder DataSeeder { get; }
    protected IConfiguration Configuration { get; }
    public PlatformServiceDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        IConfiguration configuration,
        PlatformServiceDataSeeder dataSeeder)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<PlatformServiceMigrationsDbContext>(),
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        Configuration = configuration;
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

    public async virtual Task HandleEventAsync(TenantDeletedEto eventData)
    {
        var hostDefaultConnectionString = Configuration.GetConnectionString(ConnectionStrings.DefaultConnectionStringName);
        using (CurrentTenant.Change(eventData.Id))
        {
            // 需要回收策略为回收且存在默认连接字符串且默认连接字符串与宿主不同
            if (eventData.Strategy == RecycleStrategy.Recycle && !eventData.DefaultConnectionString.IsNullOrWhiteSpace())
            {
                var hostConnection = new DbConnectionStringBuilder()
                {
                    ConnectionString = hostDefaultConnectionString,
                };
                var tenantConnection = new DbConnectionStringBuilder()
                {
                    ConnectionString = eventData.DefaultConnectionString,
                };
                if (hostConnection.EquivalentTo(tenantConnection))
                {
                    return;
                }

                using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
                var buildr = new DbContextOptionsBuilder();
                buildr.UseNpgsql(eventData.DefaultConnectionString);
                await using var dbConnection = new DbContext(buildr.Options);
                if ((await dbConnection.Database.GetAppliedMigrationsAsync()).Any())
                {
                    await dbConnection.Database.EnsureDeletedAsync();
                }
            }
        }
    }
}
