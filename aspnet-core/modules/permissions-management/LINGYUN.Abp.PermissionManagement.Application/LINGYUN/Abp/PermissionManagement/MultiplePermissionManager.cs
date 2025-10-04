using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;


namespace LINGYUN.Abp.PermissionManagement;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IMultiplePermissionManager),
    typeof(PermissionManager),
    typeof(MultiplePermissionManager))]
public class MultiplePermissionManager : PermissionManager, IMultiplePermissionManager, ISingletonDependency
{
    public MultiplePermissionManager(
        IPermissionDefinitionManager permissionDefinitionManager,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager,
        IPermissionGrantRepository permissionGrantRepository,
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
    }

    public async virtual Task SetManyAsync(string providerName, string providerKey, IEnumerable<PermissionChangeState> permissions)
    {
        // 所有权限定义
        var permissionDefinitions = await PermissionDefinitionManager.GetPermissionsAsync();

        // 存在的权限集合
        var existsPermissions = permissions
            .Join(
                permissionDefinitions,
                p => p.Name,
                pd => pd.Name,
                (p, pd) =>
                {
                    return new
                    {
                        State = p,
                        Definition = pd
                    };
                });

        // 检查权限状态
        var existsPermissionDefinitions = existsPermissions.Select(p => p.Definition).ToArray();
        var stateCheckResult = await SimpleStateCheckerManager.IsEnabledAsync(existsPermissionDefinitions);
        var invalidCheckPermissions = stateCheckResult.Where(x => !x.Value).Select(x => x.Key.Name);
        if (invalidCheckPermissions.Any())
        {
            throw new ApplicationException($"The permission named '{invalidCheckPermissions.JoinAsString(";")}' is disabled!");
        }

        // 检查权限提供者范围
        var invalidProviderPermissions = existsPermissions.Where(x => x.Definition.Providers.Any() && !x.Definition.Providers.Contains(providerName)).Select(x => x.Definition.Name);
        if (invalidProviderPermissions.Any())
        {
            throw new ApplicationException($"The permission named '{invalidProviderPermissions.JoinAsString(";")}' has not compatible with the provider named '{providerName}'");
        }
        // 检查权限多租户范围
        var multiTenancySide = CurrentTenant.GetMultiTenancySide();
        var invalidMultiTenancySidePermissions = existsPermissions.Where(x => !x.Definition.MultiTenancySide.HasFlag(multiTenancySide)).Select(x => x.Definition.Name);
        if (invalidMultiTenancySidePermissions.Any())
        {
            throw new ApplicationException($"The permission named '{invalidMultiTenancySidePermissions.JoinAsString(";")}' has multitenancy side which is not compatible with the current multitenancy side '{multiTenancySide}'");
        }

        // 获取权限提供者
        var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName) ?? 
            throw new AbpException("Unknown permission management provider: " + providerName);

        // 移除现有全部授权
        var delPermissionGrants = await PermissionGrantRepository.GetListAsync(providerName, providerKey);
        await PermissionGrantRepository.DeleteManyAsync(delPermissionGrants);

        // 重新添加授权
        var newPermissionGrants = existsPermissions
            .Where(p => p.State.IsGranted)
            .Select(p => new PermissionGrant(
                GuidGenerator.Create(),
                p.Definition.Name,
                provider.Name,
                providerKey,
                CurrentTenant.Id));
        await PermissionGrantRepository.InsertManyAsync(newPermissionGrants);
    }
}
