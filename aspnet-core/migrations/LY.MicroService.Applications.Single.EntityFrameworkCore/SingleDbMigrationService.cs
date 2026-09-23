using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore;

public class SingleDbMigrationService : EfCoreRuntimeDatabaseMigratorBase<SingleMigrationsDbContext>, ITransientDependency
{
    protected IDataSeeder DataSeeder { get; }
    public SingleDbMigrationService(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        IDataSeeder dataSeeder)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<SingleMigrationsDbContext>(), 
            unitOfWorkManager, serviceProvider, currentTenant, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task SeedAsync()
    {
        await DataSeeder.SeedAsync(new DataSeedContext());
    }
}