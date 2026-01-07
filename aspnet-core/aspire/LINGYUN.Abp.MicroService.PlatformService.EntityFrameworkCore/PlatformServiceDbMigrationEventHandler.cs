using LINGYUN.Abp.Saas.Tenants;
using LINGYUN.Abp.UI.Navigation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

namespace LINGYUN.Abp.MicroService.PlatformService;
public class PlatformServiceDbMigrationEventHandler :
    EfCoreDatabaseMigrationEventHandlerBase<PlatformServiceMigrationsDbContext>,
    IDistributedEventHandler<TenantDeletedEto>
{
    protected IEnumerable<INavigationSeedContributor> NavigationSeedContributors { get; }
    protected INavigationProvider NavigationProvider { get; }
    protected IFeatureChecker FeatureChecker { get; }
    protected IConfiguration Configuration { get; }
    public PlatformServiceDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        IFeatureChecker featureChecker,
        IConfiguration configuration,
        INavigationProvider navigationProvider,
        IEnumerable<INavigationSeedContributor> navigationSeedContributors)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<PlatformServiceMigrationsDbContext>(),
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        FeatureChecker = featureChecker;
        Configuration = configuration;
        NavigationProvider = navigationProvider;
        NavigationSeedContributors = navigationSeedContributors;
    }

    protected async override Task SeedAsync(Guid? tenantId)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var menus = await NavigationProvider.GetAllAsync();

            var multiTenancySides = CurrentTenant.IsAvailable
                ? MultiTenancySides.Tenant
                : MultiTenancySides.Host;

            var seedContext = new NavigationSeedContext(menus, multiTenancySides);

            foreach (var contributor in NavigationSeedContributors)
            {
                await contributor.SeedAsync(seedContext);
            }
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
