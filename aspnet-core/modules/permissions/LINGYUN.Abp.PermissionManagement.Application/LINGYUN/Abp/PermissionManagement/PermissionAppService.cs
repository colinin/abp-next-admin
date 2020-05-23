using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;

namespace LINGYUN.Abp.PermissionManagement
{
    [Authorize]
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IPermissionAppService), typeof(PermissionAppService))]
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        protected PermissionManagementOptions Options { get; }
        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        public PermissionAppService(
            IDistributedCache<PermissionGrantCacheItem> cache,
            IPermissionGrantRepository permissionGrantRepository,
            IPermissionDefinitionManager permissionDefinitionManager,
            IOptions<PermissionManagementOptions> options)
        {
            Cache = cache;
            Options = options.Value;
            PermissionGrantRepository = permissionGrantRepository;
            PermissionDefinitionManager = permissionDefinitionManager;
        }
        public virtual async Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
        {
            var permissionListResult = new GetPermissionListResultDto
            {
                EntityDisplayName = providerKey,
                Groups = new List<PermissionGroupDto>()
            };
            var multiTenancySide = CurrentTenant.GetMultiTenancySide();
            var permissionGroups = PermissionDefinitionManager.GetGroups();
            IEnumerable<PermissionGrant> permissions = 
                await PermissionGrantRepository.GetListAsync(providerName, providerKey);
            
            // 如果是当前用户权限,还需要查询角色权限
            if (providerName.Equals("U"))
            {
                var userId = CurrentUser.GetId().ToString();
                if (providerKey.Equals(userId))
                {
                    foreach (var role in CurrentUser.Roles)
                    {
                        var rolePermissions = await PermissionGrantRepository
                                            .GetListAsync(RolePermissionValueProvider.ProviderName, role);
                        permissions = permissions.Union(rolePermissions);
                    }
                }
            }
            foreach (var permissionGroup in permissionGroups)
            {
                var groupDto = new PermissionGroupDto
                {
                    Name = permissionGroup.Name,
                    DisplayName = permissionGroup.DisplayName.Localize(StringLocalizerFactory),
                    Permissions = new List<PermissionGrantInfoDto>()
                };
                foreach (var permission in permissionGroup.GetPermissionsWithChildren())
                {
                    if (!permission.IsEnabled)
                    {
                        continue;
                    }

                    if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
                    {
                        continue;
                    }

                    if (!permission.MultiTenancySide.HasFlag(multiTenancySide))
                    {
                        continue;
                    }

                    var grantInfoDto = new PermissionGrantInfoDto
                    {
                        Name = permission.Name,
                        DisplayName = permission.DisplayName.Localize(StringLocalizerFactory),
                        ParentName = permission.Parent?.Name,
                        AllowedProviders = permission.Providers,
                        GrantedProviders = new List<ProviderInfoDto>()
                    };

                    var grantedPermissions = permissions.Where(p => p.Name.Equals(permission.Name));

                    foreach (var grantedPermission in grantedPermissions)
                    {
                        grantInfoDto.IsGranted = true;
                        grantInfoDto.GrantedProviders.Add(new ProviderInfoDto
                        {
                            ProviderKey = grantedPermission.ProviderKey,
                            ProviderName = grantedPermission.ProviderName
                        });
                    }

                    groupDto.Permissions.Add(grantInfoDto);
                }

                if (groupDto.Permissions.Any())
                {
                    permissionListResult.Groups.Add(groupDto);
                }
            }

            return permissionListResult;
        }

        public virtual async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            await CheckProviderPolicy(providerName);

            var permissions = await PermissionGrantRepository.GetListAsync(providerName, providerKey);
            foreach(var permission in input.Permissions)
            {
                var editPermission = permissions.FirstOrDefault(p => p.Name.Equals(permission.Name));
                if(editPermission == null)
                {
                    if(permission.IsGranted)
                    {
                        var permissionGrant = new PermissionGrant(GuidGenerator.Create(), 
                            permission.Name, providerName, providerKey, CurrentTenant.Id);
                        await PermissionGrantRepository.InsertAsync(permissionGrant);
                    }
                }
                else
                {
                    if (!permission.IsGranted)
                    {
                        await PermissionGrantRepository.DeleteAsync(editPermission.Id);
                    }
                }
                // 同步变更缓存里的权限配置
                var cacheKey = CalculateCacheKey(permission.Name, providerName, providerKey);
                var cacheItem = new PermissionGrantCacheItem(permission.Name, permission.IsGranted);
                await Cache.SetAsync(cacheKey, cacheItem);
            }
        }

        protected virtual async Task CheckProviderPolicy(string providerName)
        {
            var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException($"No policy defined to get/set permissions for the provider '{policyName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
            }

            await AuthorizationService.CheckAsync(policyName);
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
