using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Saas.Tenants;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.IdentityServer.EntityFrameworkCore;

public class IdentityServerDbMigrationService : EfCoreRuntimeDbMigratorBase<IdentityServerMigrationsDbContext>, ITransientDependency
{
    protected IDataSeeder DataSeeder { get; }
    protected IDbSchemaMigrator DbSchemaMigrator { get; }
    protected ITenantRepository TenantRepository { get; }

    public IdentityServerDbMigrationService(
        IDataSeeder dataSeeder,
        IDbSchemaMigrator dbSchemaMigrator,
        ITenantRepository tenantRepository,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<IdentityServerMigrationsDbContext>(), 
            unitOfWorkManager, serviceProvider, currentTenant, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
        DbSchemaMigrator = dbSchemaMigrator;
        TenantRepository = tenantRepository;
    }

    protected async override Task LockAndApplyDatabaseMigrationsAsync()
    {
        await base.LockAndApplyDatabaseMigrationsAsync();

        var tenants = await TenantRepository.GetListAsync();
        foreach (var tenant in tenants.Where(x => x.IsActive))
        {
            await LockAndApplyDatabaseWithTenantMigrationsAsync(tenant.Id);
        }
    }

    protected async override Task SeedAsync()
    {
        Logger.LogInformation($"Executing {(!CurrentTenant.IsAvailable ? "host" : CurrentTenant.Name ?? CurrentTenant.GetId().ToString())} database seed...");

        await DataSeeder.SeedAsync(CurrentTenant.Id);
    }
}