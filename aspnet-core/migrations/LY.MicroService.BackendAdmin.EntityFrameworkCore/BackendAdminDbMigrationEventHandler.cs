using Microsoft.Extensions.Logging;
using System;
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
    protected IDataSeeder DataSeeder { get; }

    public BackendAdminDbMigrationEventHandler(
        ICurrentTenant currentTenant, 
        IUnitOfWorkManager unitOfWorkManager, 
        ITenantStore tenantStore, 
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus, 
        ILoggerFactory loggerFactory,
        IDataSeeder dataSeeder) 
        : base(
            ConnectionStringNameAttribute.GetConnStringName<BackendAdminMigrationsDbContext>(), 
            currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task SeedAsync(Guid? tenantId)
    {
        await DataSeeder.SeedAsync(tenantId);   
    }
}
