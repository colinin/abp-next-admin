using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MultiTenancy.DbFinder.EventBus.Distributed
{
    public class TenantUpdateEventHandler : IDistributedEventHandler<EntityUpdatedEto<TenantEto>>, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenantRepository _tenantRepository;
        private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;

        public TenantUpdateEventHandler(
            ICurrentTenant currentTenant,
            ITenantRepository tenantRepository,
            IDistributedCache<TenantConfigurationCacheItem> cache)
        {
            _cache = cache;
            _currentTenant = currentTenant;
            _tenantRepository = tenantRepository;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(EntityUpdatedEto<TenantEto> eventData)
        {
            using (_currentTenant.Change(null))
            {
                var tenant = await _tenantRepository.FindAsync(eventData.Entity.Id, true);
                if (tenant == null)
                {
                    return;
                }
                var connectionStrings = new ConnectionStrings();
                foreach (var tenantConnectionString in tenant.ConnectionStrings)
                {
                    connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
                }
                var cacheItem = new TenantConfigurationCacheItem(tenant.Id, tenant.Name, connectionStrings);

                var cacheKey = TenantConfigurationCacheItem.CalculateCacheKey(eventData.Entity.Id.ToString());
                await _cache.SetAsync(cacheKey, cacheItem);
            }
        }
    }
}
