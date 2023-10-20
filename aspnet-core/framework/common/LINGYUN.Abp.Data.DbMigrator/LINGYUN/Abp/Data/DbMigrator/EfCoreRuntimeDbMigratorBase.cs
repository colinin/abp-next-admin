using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Data.DbMigrator;
public abstract class EfCoreRuntimeDbMigratorBase<TDbContext> : EfCoreRuntimeDatabaseMigratorBase<TDbContext>
    where TDbContext : DbContext, IEfCoreDbContext
{
    protected EfCoreRuntimeDbMigratorBase(
        string databaseName,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory)
        : base(databaseName, unitOfWorkManager, serviceProvider, currentTenant, abpDistributedLock, distributedEventBus, loggerFactory)
    {
    }

    protected async virtual Task LockAndApplyDatabaseWithTenantMigrationsAsync(Guid tenantId)
    {
        Logger.LogInformation($"Trying to acquire the distributed lock for database migration: {DatabaseName} with tenant: {tenantId}.");

        var schemaMigrated = false;

        await using (var handle = await DistributedLock.TryAcquireAsync("DatabaseMigration_" + DatabaseName + "_Tenant" + tenantId.ToString()))
        {
            if (handle is null)
            {
                Logger.LogInformation($"Distributed lock could not be acquired for database migration: {DatabaseName} with tenant: {tenantId}. Operation cancelled.");
                return;
            }

            Logger.LogInformation($"Distributed lock is acquired for database migration: {DatabaseName} with tenant: {tenantId}...");

            using (CurrentTenant.Change(tenantId))
            {
                // Create database tables if needed
                using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
                var dbContext = await ServiceProvider
                    .GetRequiredService<IDbContextProvider<TDbContext>>()
                    .GetDbContextAsync();

                var pendingMigrations = await dbContext
                    .Database
                    .GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync();
                    schemaMigrated = true;
                }

                await uow.CompleteAsync();

                await SeedAsync();
            }

            if (schemaMigrated || AlwaysSeedTenantDatabases)
            {
                await DistributedEventBus.PublishAsync(
                    new AppliedDatabaseMigrationsEto
                    {
                        DatabaseName = DatabaseName,
                        TenantId = null
                    }
                );
            }
        }

        Logger.LogInformation($"Distributed lock has been released for database migration: {DatabaseName} with tenant: {tenantId}...");
    }
}
