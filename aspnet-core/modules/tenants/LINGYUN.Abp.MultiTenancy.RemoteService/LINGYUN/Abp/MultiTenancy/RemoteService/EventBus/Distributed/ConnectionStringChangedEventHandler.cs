using LINGYUN.Abp.TenantManagement;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.MultiTenancy.RemoteService.EventBus.Distributed
{
    public class ConnectionStringChangedEventHandler : IDistributedEventHandler<ConnectionStringChangedEventData>, ITransientDependency
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;

        public ConnectionStringChangedEventHandler(
            ITenantAppService tenantAppService,
            IDistributedCache<TenantConfigurationCacheItem> cache)
        {
            _cache = cache;
            _tenantAppService = tenantAppService;
        }


        public virtual async Task HandleEventAsync(ConnectionStringChangedEventData eventData)
        {
            var tenantDto = await _tenantAppService.GetAsync(eventData.Id);
            var tenantConnectionStringsDto = await _tenantAppService.GetConnectionStringAsync(eventData.Id);
            var connectionStrings = new ConnectionStrings();
            foreach (var tenantConnectionString in tenantConnectionStringsDto.Items)
            {
                connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
            }
            var cacheItem = new TenantConfigurationCacheItem(tenantDto.Id, tenantDto.Name, connectionStrings);

            await _cache.SetAsync(
                TenantConfigurationCacheItem.CalculateCacheKey(eventData.Id.ToString()), 
                cacheItem);

            await _cache.SetAsync(
                TenantConfigurationCacheItem.CalculateCacheKey(tenantDto.Name),
                cacheItem);
        }
    }
}
