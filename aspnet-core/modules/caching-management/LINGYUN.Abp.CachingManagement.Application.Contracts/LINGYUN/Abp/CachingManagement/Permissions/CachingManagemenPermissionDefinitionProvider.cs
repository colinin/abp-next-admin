using LINGYUN.Abp.CachingManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.CachingManagement.Permissions;

public class CachingManagemenPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var cachingManagerGroup = context.AddGroup(
            CachingManagementPermissionNames.GroupName, 
            L("Permission:CachingManagement"));

        var cacheGroup = cachingManagerGroup.AddPermission(CachingManagementPermissionNames.Cache.Default, L("Permission:Caches"));
        cacheGroup.AddChild(CachingManagementPermissionNames.Cache.ManageValue, L("Permission:ManageValue"));
        cacheGroup.AddChild(CachingManagementPermissionNames.Cache.Refresh, L("Permission:Refresh"));
        cacheGroup.AddChild(CachingManagementPermissionNames.Cache.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CacheResource>(name);
    }
}
