using LINGYUN.Abp.Saas.Tenants;
using LY.MicroService.Applications.Single.MultiTenancy;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace LY.MicroService.Applications.Single.EventBus.Distributed
{
    public class TenantEventHandler : 
        IDistributedEventHandler<EntityCreatedEto<TenantEto>>,
        IDistributedEventHandler<EntityUpdatedEto<TenantEto>>,
        IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
        IDistributedEventHandler<TenantConnectionStringUpdatedEto>,
        ITransientDependency
    {
        protected ITenantConfigurationCache TenantConfigurationCache { get; }

        public TenantEventHandler(
            ITenantConfigurationCache tenantConfigurationCache)
        {
            TenantConfigurationCache = tenantConfigurationCache;
        }

        [UnitOfWork]
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
