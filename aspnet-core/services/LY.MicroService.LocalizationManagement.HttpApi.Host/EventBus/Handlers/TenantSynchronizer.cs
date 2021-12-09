using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.MultiTenancy;
using LY.MicroService.LocalizationManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.LocalizationManagement.EventBus.Handlers
{
    public class TenantSynchronizer : IDistributedEventHandler<CreateEventData>, ITransientDependency
    {
        protected ILogger<TenantSynchronizer> Logger { get; }

        protected ICurrentTenant CurrentTenant { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IDbSchemaMigrator DbSchemaMigrator { get; }
        public TenantSynchronizer(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IDbSchemaMigrator dbSchemaMigrator,
            ILogger<TenantSynchronizer> logger)
        {
            CurrentTenant = currentTenant;
            UnitOfWorkManager = unitOfWorkManager;
            DbSchemaMigrator = dbSchemaMigrator;

            Logger = logger;
        }

        public async Task HandleEventAsync(CreateEventData eventData)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                using (CurrentTenant.Change(eventData.Id, eventData.Name))
                {
                    Logger.LogInformation("Migrating the new tenant database with localization..");
                    // 迁移租户数据
                    await DbSchemaMigrator.MigrateAsync<LocalizationManagementMigrationsDbContext>(
                        (connectionString, builder) =>
                        {
                            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                            return new LocalizationManagementMigrationsDbContext(builder.Options);
                        });
                    await unitOfWork.SaveChangesAsync();

                    Logger.LogInformation("Migrated the new tenant database with localization.");
                }
            }
        }
    }
}
