using LINGYUN.Abp.Data.DbMigrator;
using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MicroService.LocalizationService;
public class LocalizationServiceDbMigrationService : EfCoreRuntimeDbMigratorBase<LocalizationServiceMigrationsDbContext>, ITransientDependency
{
    public LocalizationServiceDbMigrationService(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory)
        : base(
            ConnectionStringNameAttribute.GetConnStringName<LocalizationServiceMigrationsDbContext>(),
            unitOfWorkManager, serviceProvider, currentTenant, abpDistributedLock, distributedEventBus, loggerFactory)
    {
    }
}