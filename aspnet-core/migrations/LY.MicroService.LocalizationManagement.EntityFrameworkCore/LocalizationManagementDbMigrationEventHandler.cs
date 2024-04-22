using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.LocalizationManagement.EntityFrameworkCore;
public class LocalizationManagementDbMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<LocalizationManagementMigrationsDbContext>
{
    public LocalizationManagementDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory) 
        : base(
            ConnectionStringNameAttribute.GetConnStringName<LocalizationManagementMigrationsDbContext>(), 
            currentTenant, unitOfWorkManager, tenantStore, distributedEventBus, loggerFactory)
    {
    }
}
