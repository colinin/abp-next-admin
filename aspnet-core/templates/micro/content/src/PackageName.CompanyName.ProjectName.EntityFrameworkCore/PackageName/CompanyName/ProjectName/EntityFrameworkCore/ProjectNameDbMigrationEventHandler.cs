using Microsoft.Extensions.Logging;
using System;
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
    protected IDataSeeder DataSeeder { get; }

    public ProjectNameDbMigrationEventHandler(
        IDataSeeder dataSeeder,
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

    protected async override Task SeedAsync(Guid? tenantId)
    {
        await DataSeeder.SeedAsync(tenantId);
    }
}
