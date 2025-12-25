using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.SystemInfo.Permissions;

public class SystemInfoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var systemInfoGroup = context.AddGroup(
            SystemInfoPermissions.GroupName,
            new FixedLocalizableString("系统详情"));

        systemInfoGroup.AddPermission(
            SystemInfoPermissions.Default,
            new FixedLocalizableString("获取系统详情"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .WithProviders(ClientPermissionValueProvider.ProviderName); // 使用客户端授权
    }
}
