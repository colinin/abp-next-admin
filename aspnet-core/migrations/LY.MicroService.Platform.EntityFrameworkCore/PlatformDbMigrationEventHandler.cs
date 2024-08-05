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
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.Platform.EntityFrameworkCore;
public class PlatformDbMigrationEventHandler : 
    EfCoreDatabaseMigrationEventHandlerBase<PlatformMigrationsDbContext>,
    IDistributedEventHandler<TenantDeletedEto>
{
    protected IDataSeeder DataSeeder { get; }
    protected IFeatureChecker FeatureChecker { get; }
    protected IConfiguration Configuration { get; }
    public PlatformDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        IDataSeeder dataSeeder,
        IFeatureChecker featureChecker,
        IConfiguration configuration)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<PlatformMigrationsDbContext>(), 
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
        FeatureChecker = featureChecker;
        Configuration = configuration;
    }

    protected async override Task SeedAsync(Guid? tenantId)
    {
        using (CurrentTenant.Change(tenantId))
        {
            await DataSeeder.SeedAsync(tenantId);
        }
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
                buildr.UseMySql(eventData.DefaultConnectionString, ServerVersion.AutoDetect(eventData.DefaultConnectionString));
                await using var dbConnection = new DbContext(buildr.Options);
                if ((await dbConnection.Database.GetAppliedMigrationsAsync()).Any())
                {
                    await dbConnection.Database.EnsureDeletedAsync();
                }
            }
        }
    }
}
