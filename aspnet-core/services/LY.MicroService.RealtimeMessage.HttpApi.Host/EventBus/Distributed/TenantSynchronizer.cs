using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.Saas.Tenants;
using LY.MicroService.RealtimeMessage.MultiTenancy;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.RealtimeMessage.EventBus.Distributed
{
    public class TenantSynchronizer : 
        IDistributedEventHandler<EntityCreatedEto<TenantEto>>,
        IDistributedEventHandler<EntityUpdatedEto<TenantEto>>,
        IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
        IDistributedEventHandler<TenantConnectionStringUpdatedEto>,
        ITransientDependency
    {
        protected ILogger<TenantSynchronizer> Logger { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IDbSchemaMigrator DbSchemaMigrator { get; }
        protected INotificationSender NotificationSender { get; }
        protected ITenantConfigurationCache TenantConfigurationCache { get; }
        protected INotificationSubscriptionManager NotificationSubscriptionManager { get; }

        public TenantSynchronizer(
            ICurrentTenant currentTenant,
            IDbSchemaMigrator dbSchemaMigrator,
            IUnitOfWorkManager unitOfWorkManager,
            INotificationSender notificationSender,
            ITenantConfigurationCache tenantConfigurationCache,
            INotificationSubscriptionManager notificationSubscriptionManager,
            ILogger<TenantSynchronizer> logger)
        {
            Logger = logger;

            CurrentTenant = currentTenant;
            DbSchemaMigrator = dbSchemaMigrator;
            UnitOfWorkManager = unitOfWorkManager;
            TenantConfigurationCache = tenantConfigurationCache;

            NotificationSender = notificationSender;
            NotificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async virtual Task HandleEventAsync(EntityCreatedEto<TenantEto> eventData)
        {
            await TenantConfigurationCache.RefreshAsync();
        }

        public async virtual Task HandleEventAsync(EntityUpdatedEto<TenantEto> eventData)
        {
            await TenantConfigurationCache.RefreshAsync();
        }

        public async virtual Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
        {
            await TenantConfigurationCache.RefreshAsync();
        }

        public async virtual Task HandleEventAsync(TenantConnectionStringUpdatedEto eventData)
        {
            await TenantConfigurationCache.RefreshAsync();
        }
    }
}
