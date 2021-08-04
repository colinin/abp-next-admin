using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.PermissionManagement
{
    [Dependency(ServiceLifetime.Singleton, ReplaceServices = true)]
    [ExposeServices(typeof(IPermissionManager), typeof(PermissionManager))]
    public class DefaultPermissionManager : PermissionManager
    {
        #region DependencyInjection

        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }
        protected IServiceProvider ServiceProvider { get; }

        #endregion

        protected IPermissionStore PermissionStore { get; }
        public DefaultPermissionManager(
            IPermissionDefinitionManager permissionDefinitionManager,
            ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager,
            IPermissionGrantRepository permissionGrantRepository,
            IPermissionStore permissionStore,
            IServiceProvider serviceProvider, 
            IGuidGenerator guidGenerator, 
            IOptions<PermissionManagementOptions> options, 
            ICurrentTenant currentTenant,
            IDistributedCache<PermissionGrantCacheItem> cache) 
            : base(
                  permissionDefinitionManager,
                  simpleStateCheckerManager, 
                  permissionGrantRepository,
                  serviceProvider, 
                  guidGenerator, 
                  options, 
                  currentTenant,
                  cache)
        {
            ServiceProvider = serviceProvider;
            PermissionStore = permissionStore;
        }

        public override async Task SetAsync(string permissionName, string providerName, string providerKey, bool isGranted)
        {
            await base.SetAsync(permissionName, providerName, providerKey, isGranted);

            // 不需要改缓存,因为权限实体变更会自动清理缓存
            //var cacheKey = PermissionGrantCacheItem.CalculateCacheKey(permissionName, providerName, providerKey);
            //var cacheItem = new PermissionGrantCacheItem(isGranted);
            //await PermissionGrantChche.SetAsync(cacheKey, cacheItem);
        }

        protected override async Task<PermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition permission, string providerName, string providerKey)
        {
            var result = new PermissionWithGrantedProviders(permission.Name, false);

            if (!permission.IsEnabled)
            {
                return result;
            }

            if (!await SimpleStateCheckerManager.IsEnabledAsync(permission))
            {
                return result;
            }

            if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
            {
                return result;
            }

            if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
            {
                return result;
            }

            // 这么做的坏处就是没法给特定的Provider设定是否授权字段
            // result.Providers 会出现假数据 UserPermissionProvider未授权, 而所属的

            result.IsGranted = await IsGrantedAsync(permission.Name, providerName, providerKey);

            return result;
        }

        protected virtual async Task<bool> IsGrantedAsync(string permissionName, string providerName, string providerKey)
        {
            return await PermissionStore.IsGrantedAsync(permissionName, providerName, providerKey);
        }
    }
}
