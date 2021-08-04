using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.PermissionManagement.Identity
{
    [Dependency(ServiceLifetime.Singleton, ReplaceServices = true)]
    [ExposeServices(typeof(IPermissionManager), typeof(PermissionManager), typeof(DefaultPermissionManager))]
    public class IdentityPermissionManager : DefaultPermissionManager
    {
        protected IUnitOfWorkManager UnitOfWorkManager => LazyGetRequiredService(ref _unitOfWorkManager);
        private IUnitOfWorkManager _unitOfWorkManager;

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        protected IUserRoleFinder UserRoleFinder { get; }
        public IdentityPermissionManager(
            IPermissionDefinitionManager permissionDefinitionManager,
            ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager,
            IPermissionGrantRepository permissionGrantRepository, 
            IPermissionStore permissionStore, 
            IServiceProvider serviceProvider, 
            IGuidGenerator guidGenerator, 
            IOptions<PermissionManagementOptions> options, 
            ICurrentTenant currentTenant,
             IDistributedCache<PermissionGrantCacheItem> cache,
            IUserRoleFinder userRoleFinder) 
            : base(
                  permissionDefinitionManager,
                  simpleStateCheckerManager, 
                  permissionGrantRepository, 
                  permissionStore, 
                  serviceProvider, 
                  guidGenerator, 
                  options, 
                  currentTenant, 
                  cache)
        {
            UserRoleFinder = userRoleFinder;
        }

        protected override async Task<bool> IsGrantedAsync(string permissionName, string providerName, string providerKey)
        {
            if (!RolePermissionValueProvider.ProviderName.Equals(providerName))
            {
                // 如果查询的是用户权限,需要查询用户角色权限
                if (providerName == UserPermissionValueProvider.ProviderName)
                {
                    var userId = Guid.Parse(providerKey);
                    var roleNames = await GetUserRolesAsync(userId);
                    foreach (var roleName in roleNames)
                    {
                        var permissionGrant = await PermissionStore.IsGrantedAsync(permissionName, RolePermissionValueProvider.ProviderName, roleName);
                        if (permissionGrant)
                        {
                            return true;
                        }
                    }
                }
            }
            return await base.IsGrantedAsync(permissionName, providerName, providerKey);
        }

        protected virtual async Task<string[]> GetUserRolesAsync(Guid userId)
        {
            // 通过工作单元来缓存用户角色,防止多次查询
            if (CurrentUnitOfWork != null)
            {
                var userRoleItemKey = $"FindRolesByUser:{userId}";

                return await CurrentUnitOfWork.GetOrAddItem(userRoleItemKey, (key) =>
                {
                    // 取消同步调用
                    //var roles = AsyncHelper.RunSync(async ()=> await UserRoleFinder.GetRolesAsync(userId));
                    return UserRoleFinder.GetRolesAsync(userId);
                });
            }
            return await UserRoleFinder.GetRolesAsync(userId);
        }
    }
}
