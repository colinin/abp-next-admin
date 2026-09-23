using LINGYUN.Abp.Data.DbMigrator;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.RealtimeMessage.EntityFrameworkCore;

public class RealtimeMessageDbMigrationService : EfCoreRuntimeDbMigratorBase<RealtimeMessageMigrationsDbContext>, ITransientDependency
{
    protected RealtimeMessageDataSeeder DataSeeder { get; }

    public RealtimeMessageDbMigrationService(
        RealtimeMessageDataSeeder dataSeeder,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<RealtimeMessageMigrationsDbContext>(),
            unitOfWorkManager, serviceProvider, currentTenant, abpDistributedLock, distributedEventBus, loggerFactory)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task SeedAsync()
    {
        // DbMigrator迁移数据种子
        await DataSeeder.SeedAsync(new DataSeedContext());
    }
}